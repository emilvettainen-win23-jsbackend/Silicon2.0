using System.Text.Json;
using System.Text;
using Presentation.BlazorApp.Models.Courses;
using GraphQL;
using Microsoft.Azure.Amqp.Framing;

namespace Presentation.BlazorApp.GraphQL;



public class GraphQLService
{
    private readonly HttpClient _httpClient;
    private readonly string _graphqlEndpoint;
   

    public GraphQLService(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _graphqlEndpoint = configuration["URI:COURSE_PROVIDER"]!; // Should ideally be taken from configuration
    }

    public async Task<List<CourseBoxModel>> GetAllCoursesAsync()
    {
        var query = @"
        {
             getAllCourses {
                id
                courseImageUrl
                courseTitle
                isBestseller
                category
                created

                rating {
                    inNumbers
                    inProcent
                }
                prices {
                    originalPrice
                    discountPrice
                }
                included {
                    hoursOfVideo
                }
                author {
                    fullName
                }
            }
        }";
        var request = new { query, variables = new { } };
        var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(responseContent);
        var root = document.RootElement;

        // readfromjson <dynamicgrapqlresponse> 
        // JsonPropertyName("data") JsonElement Data get: set;
        // result.data.getproperty 



        var courses = new List<CourseBoxModel>();

        if (root.GetProperty("data").TryGetProperty("getAllCourses", out var courseArray))
        {
            foreach (var courseElement in courseArray.EnumerateArray())
            {
                var course = new CourseBoxModel
                {
                    Id = courseElement.GetProperty("id").GetString() ?? "",
                    CourseImageUrl = courseElement.GetProperty("courseImageUrl").GetString() ?? "",
                    CourseTitle = courseElement.GetProperty("courseTitle").GetString() ?? "",
                    IsBestseller = courseElement.GetProperty("isBestseller").GetBoolean(),
                    Category = courseElement.GetProperty("category").GetString() ?? "",
                    Created = courseElement.GetProperty("created").GetDateTime(),
                    Rating = new RatingBoxModel
                    {
                        InNumbers = courseElement.GetProperty("rating").GetProperty("inNumbers").GetDecimal(),
                        InProcent = courseElement.GetProperty("rating").GetProperty("inProcent").GetDecimal()
                    },
                    Prices = new PriceBoxModel
                    {
                        OriginalPrice = courseElement.GetProperty("prices").GetProperty("originalPrice").GetDecimal(),
                        DiscountPrice = courseElement.GetProperty("prices").GetProperty("discountPrice").ValueKind != JsonValueKind.Null
                            ? courseElement.GetProperty("prices").GetProperty("discountPrice").GetDecimal()
                            : (decimal?)null
                    },
                    Included = new IncludedBoxModel
                    {
                        HoursOfVideo = courseElement.GetProperty("included").GetProperty("hoursOfVideo").GetInt32()
                    },
                    Author = new AuthorBoxModel
                    {
                        FullName = courseElement.GetProperty("author").GetProperty("fullName").GetString() ?? ""
                    }
                };
                courses.Add(course);
            }
        }
        return courses.OrderByDescending(c => c.Created).ToList();
    }


    public async Task<List<CourseBoxModel>> GetAllCoursesByCategoryAsync(string category)
    {
        var query = @"
            query($category: String!) {
                getCoursesByCategory(category: $category) {
                    id
                    courseImageUrl
                    courseTitle
                    isBestseller
                    category
                    rating {
                        inNumbers
                        inProcent
                    }
                    prices {
                        originalPrice
                        discountPrice
                    }
                    included {
                        hoursOfVideo
                    }
                    author {
                        fullName
                    }
                }
            }";

        var request = new
        {
            query,
            variables = new { category }
        };

        var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(responseContent);
        var root = document.RootElement;

        var courses = new List<CourseBoxModel>();

        if (root.GetProperty("data").TryGetProperty("getCoursesByCategory", out var courseArray))
        {
            foreach (var courseElement in courseArray.EnumerateArray())
            {
                var course = new CourseBoxModel
                {
                    Id = courseElement.GetProperty("id").GetString() ?? "",
                    CourseImageUrl = courseElement.GetProperty("courseImageUrl").GetString() ?? "",
                    CourseTitle = courseElement.GetProperty("courseTitle").GetString() ?? "",
                    IsBestseller = courseElement.GetProperty("isBestseller").GetBoolean(),
                    Category = courseElement.GetProperty("category").GetString() ?? "",
                    Rating = new RatingBoxModel
                    {
                        InNumbers = courseElement.GetProperty("rating").GetProperty("inNumbers").GetDecimal(),
                        InProcent = courseElement.GetProperty("rating").GetProperty("inProcent").GetDecimal()
                    },
                    Prices = new PriceBoxModel
                    {
                        OriginalPrice = courseElement.GetProperty("prices").GetProperty("originalPrice").GetDecimal(),
                        DiscountPrice = courseElement.GetProperty("prices").GetProperty("discountPrice").ValueKind != JsonValueKind.Null
                            ? courseElement.GetProperty("prices").GetProperty("discountPrice").GetDecimal()
                            : (decimal?)null
                    },
                    Included = new IncludedBoxModel
                    {
                        HoursOfVideo = courseElement.GetProperty("included").GetProperty("hoursOfVideo").GetInt32()
                    },
                    Author = new AuthorBoxModel
                    {
                        FullName = courseElement.GetProperty("author").GetProperty("fullName").GetString() ?? ""
                    }
                };

                courses.Add(course);
            }
        }

        return courses;
    }


    public async Task<List<string>> GetAllCategoriesAsync()
    {
        var query = @"
            {
                getAllCategories
            }";

        var request = new { query };

        var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(responseContent);
        var root = document.RootElement;

        var categories = new List<string>();

        if (root.GetProperty("data").TryGetProperty("getAllCategories", out var categoryArray))
        {
            foreach (var categoryElement in categoryArray.EnumerateArray())
            {
                categories.Add(categoryElement.GetString() ?? "");
            }
        }

        return categories;
    }


    public async Task<List<CourseBoxModel>> SearchCoursesAsync(string searchQuery)
    {
        var query = @"
    query($searchQuery: String!) {
        getCoursesBySearch(searchQuery: $searchQuery) {
            id
            courseImageUrl
            courseTitle
            isBestseller
            category
            rating {
                inNumbers
                inProcent
            }
            prices {
                originalPrice
                discountPrice
            }
            included {
                hoursOfVideo
            }
            author {
                fullName
            }
        }
    }";

        var request = new { query, variables = new { searchQuery } };

        var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(responseContent);
        var root = document.RootElement;

        var courses = new List<CourseBoxModel>();

        if (root.GetProperty("data").TryGetProperty("getCoursesBySearch", out var courseArray))
        {
            foreach (var courseElement in courseArray.EnumerateArray())
            {
                var course = new CourseBoxModel
                {
                    Id = courseElement.GetProperty("id").GetString() ?? "",
                    CourseImageUrl = courseElement.GetProperty("courseImageUrl").GetString() ?? "",
                    CourseTitle = courseElement.GetProperty("courseTitle").GetString() ?? "",
                    IsBestseller = courseElement.GetProperty("isBestseller").GetBoolean(),
                    Category = courseElement.GetProperty("category").GetString() ?? "",
                    Rating = new RatingBoxModel
                    {
                        InNumbers = courseElement.GetProperty("rating").GetProperty("inNumbers").GetDecimal(),
                        InProcent = courseElement.GetProperty("rating").GetProperty("inProcent").GetDecimal()
                    },
                    Prices = new PriceBoxModel
                    {
                        OriginalPrice = courseElement.GetProperty("prices").GetProperty("originalPrice").GetDecimal(),
                        DiscountPrice = courseElement.GetProperty("prices").GetProperty("discountPrice").ValueKind != JsonValueKind.Null
                            ? courseElement.GetProperty("prices").GetProperty("discountPrice").GetDecimal()
                            : (decimal?)null
                    },
                    Included = new IncludedBoxModel
                    {
                        HoursOfVideo = courseElement.GetProperty("included").GetProperty("hoursOfVideo").GetInt32()
                    },
                    Author = new AuthorBoxModel
                    {
                        FullName = courseElement.GetProperty("author").GetProperty("fullName").GetString() ?? ""
                    }
                };

                courses.Add(course);
            }
        }

        return courses;
    }


    public async Task<CourseModel> GetCourseByIdAsync(string id)
    {
        var query = @"
    query($id: String!) {
    getCourseById(id: $id) {
        id
        courseTitle
        courseIngress
        courseDescription
        courseImageUrl
        isBestseller
        category
        created
        lastUpdated
        rating {
            inNumbers
            inProcent
        }
        prices {
            originalPrice
            discountPrice
        }
        included {
            hoursOfVideo
            articles
            resources
            lifetimeAccess
            certificate
        }
        author {
            fullName
            biography
            profileImageUrl
            socialMedia {
                youTubeUrl
                subscribers
                facebookUrl
                followers
            }
        }
        highlights {
            highlight
        }
        content {
            title
            description
        }
    }
}";

        var request = new { query, variables = new { id } };

        var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
   
        var document = JsonDocument.Parse(responseContent);
        var root = document.RootElement;

        if (root.GetProperty("data").TryGetProperty("getCourseById", out var courseElement))
        {
            var course = new CourseModel
            {
                Id = courseElement.GetProperty("id").GetString() ?? "",
                CourseTitle = courseElement.GetProperty("courseTitle").GetString() ?? "",
                CourseIngress = courseElement.GetProperty("courseIngress").GetString() ?? "",
                CourseDescription = courseElement.GetProperty("courseDescription").GetString() ?? "",
                CourseImageUrl = courseElement.GetProperty("courseImageUrl").GetString() ?? "",
                IsBestseller = courseElement.GetProperty("isBestseller").GetBoolean(),
                CourseCategory = courseElement.GetProperty("category").GetString() ?? "",
                Created = courseElement.GetProperty("created").GetDateTime(),
                LastUpdated = courseElement.GetProperty("lastUpdated").GetDateTime(),
                Rating = courseElement.TryGetProperty("rating", out var ratingElement) ? new RatingModel
                {
                    InNumbers = ratingElement.GetProperty("inNumbers").GetDecimal(),
                    InProcent = ratingElement.GetProperty("inProcent").GetDecimal()
                } : null!,
                Price = courseElement.TryGetProperty("prices", out var priceElement) ? new PriceModel
                {
                    OriginalPrice = priceElement.GetProperty("originalPrice").GetDecimal(),
                    DiscountPrice = priceElement.TryGetProperty("discountPrice", out var discountElement) && discountElement.ValueKind != JsonValueKind.Null
                        ? discountElement.GetDecimal()
                        : (decimal?)null
                } : null!,
                Included = courseElement.TryGetProperty("included", out var includedElement) ? new IncludedModel
                {
                    HoursOfVideo = includedElement.GetProperty("hoursOfVideo").GetInt32(),
                    Articles = includedElement.GetProperty("articles").GetInt32(),
                    Resourses = includedElement.GetProperty("resources").GetInt32(),
                    LifetimeAccess = includedElement.GetProperty("lifetimeAccess").GetBoolean(),
                    Certificate = includedElement.GetProperty("certificate").GetBoolean()
                } : null!,
                Author = courseElement.TryGetProperty("author", out var authorElement) ? new AuthorModel
                {
                    FullName = authorElement.GetProperty("fullName").GetString() ?? "",
                    Biography = authorElement.GetProperty("biography").GetString() ?? "",
                    ProfileImageUrl = authorElement.GetProperty("profileImageUrl").GetString() ?? "",
                    SocialMedia = authorElement.TryGetProperty("socialMedia", out var socialMediaElement) ? new SocialMediaModel
                    {
                        YouTubeUrl = socialMediaElement.GetProperty("youTubeUrl").GetString(),
                        Subscribers = socialMediaElement.GetProperty("subscribers").GetString(),
                        FacebookUrl = socialMediaElement.GetProperty("facebookUrl").GetString(),
                        Followers = socialMediaElement.GetProperty("followers").GetString()
                    } : null
                } : null!,
                Highlights = courseElement.TryGetProperty("highlights", out var highlightsElement) ? highlightsElement.EnumerateArray().Select(highlightElement => new HighlightsModel
                {
                    Highlight = highlightElement.GetProperty("highlight").GetString() ?? ""
                }).ToList() : new List<HighlightsModel>(),
                Content = courseElement.TryGetProperty("content", out var contentElement) ? contentElement.EnumerateArray().Select(contentElement => new ProgramDetailsModel
                {
                    Title = contentElement.GetProperty("title").GetString() ?? "",
                    Description = contentElement.GetProperty("description").GetString() ?? ""
                }).ToList() : new List<ProgramDetailsModel>()
            };

            return course;
        }

        return new CourseModel();
    }



    public async Task<List<CourseBoxModel>> GetCoursesByIdsAsync(IEnumerable<string> ids)
    {
        var query = @"query($ids: [String!]!) { getCoursesByIds(ids: $ids) 
        {
            id
            courseImageUrl
            courseTitle
            isBestseller
            category
            rating { inNumbers inProcent }
            prices { originalPrice discountPrice }
            included { hoursOfVideo }
            author { fullName }
        }}";

        var request = new { query, variables = new { ids } };

        var response = await _httpClient.PostAsJsonAsync(_graphqlEndpoint, request);

        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        var document = JsonDocument.Parse(responseContent);
        var root = document.RootElement;

        var courses = new List<CourseBoxModel>();

        if (root.GetProperty("data").TryGetProperty("getCoursesByIds", out var courseArray))
        {
            foreach (var courseElement in courseArray.EnumerateArray())
            {
                var course = new CourseBoxModel
                {
                    Id = courseElement.GetProperty("id").GetString() ?? "",
                    CourseImageUrl = courseElement.GetProperty("courseImageUrl").GetString() ?? "",
                    CourseTitle = courseElement.GetProperty("courseTitle").GetString() ?? "",
                    IsBestseller = courseElement.GetProperty("isBestseller").GetBoolean(),
                    Category = courseElement.GetProperty("category").GetString() ?? "",
                    Rating = new RatingBoxModel
                    {
                        InNumbers = courseElement.GetProperty("rating").GetProperty("inNumbers").GetDecimal(),
                        InProcent = courseElement.GetProperty("rating").GetProperty("inProcent").GetDecimal()
                    },
                    Prices = new PriceBoxModel
                    {
                        OriginalPrice = courseElement.GetProperty("prices").GetProperty("originalPrice").GetDecimal(),
                        DiscountPrice = courseElement.GetProperty("prices").GetProperty("discountPrice").ValueKind != JsonValueKind.Null
                            ? courseElement.GetProperty("prices").GetProperty("discountPrice").GetDecimal()
                            : (decimal?)null
                    },
                    Included = new IncludedBoxModel
                    {
                        HoursOfVideo = courseElement.GetProperty("included").GetProperty("hoursOfVideo").GetInt32()
                    },
                    Author = new AuthorBoxModel
                    {
                        FullName = courseElement.GetProperty("author").GetProperty("fullName").GetString() ?? ""
                    }
                };

                courses.Add(course);
            }
        }

        return courses;



    }
}