using Microsoft.EntityFrameworkCore;
using Modules.Blog.Features.Entities;
using Shared.Features.Persistence;
using System.Security.Cryptography;

namespace Modules.Blog.Features.Persistence;


public class BlogContextSeed : IContextSeed
{
	public static void Seed(DbContext context, bool _)
	{
		if (context.Set<Post>().Any())
			return;

		var tags = GetTags();

		var posts = new List<Post>();
		for (int i = 0; i < 15; i++)
		{
			var title = GetTitle();
			var publishedDate = DateTime.UtcNow.AddMinutes(RandomNumberGenerator.GetInt32(0, 360) * -1);
			posts.Add(new Post
			{
				Title = title,
				Slug = Post.CreateSlug(title),
				Description = GetDescription(),
				IsPublished = true,
				PublishedDate = publishedDate,
				UpdatedDate = publishedDate,
				Tags = tags.OrderBy(x => RandomNumberGenerator.GetInt32(int.MaxValue)).Take(RandomNumberGenerator.GetInt32(1, 6)).ToList(),
				Comments = GetComments(),
				Body = GetBody(),
			});
		}

		context.Set<Post>().AddRange(posts);
		context.SaveChanges();
	}

	static List<Tag> GetTags()
	{
		var tags = new List<string>()
		{
			"C#", ".Net", "Blazor", "ASP.NET", "SQL Server", "DDD", "Angular", "MVC", "React", "Javascript", "Typescript", "Challanges", "Algorithem"
		}
		.Select(x => new Tag { Name = x })
		.ToList();

		return tags;
	}

	static string GetTitle()
	{
		var blogPostTitles = new List<string>
		{
			"Mastering the Basics of C#",
			"Exploring the Power of Python for Data Science",
			"10 Tips for Writing Clean Code",
			"Understanding Object-Oriented Programming",
			"Getting Started with Web Development in JavaScript",
			"The Future of AI and Machine Learning",
			"Building Mobile Apps with Flutter",
			"A Beginner's Guide to Version Control with Git",
			"Optimizing Performance in .NET Applications",
			"Introduction to Functional Programming",
			"Creating Responsive Web Designs with CSS Grid",
			"Exploring the World of Game Development with Unity",
			"Best Practices for Secure Coding",
			"Harnessing the Power of Cloud Computing with Azure",
			"Debugging Techniques for Efficient Problem Solving"
		}
		.OrderBy(x => RandomNumberGenerator.GetInt32(int.MaxValue))
		.ToList();

		return blogPostTitles.First();
	}

	static string GetDescription()
	{
		var blogDescriptions = new List<string>
		{
			"Discover the latest trends in web development and boost your skills with our comprehensive guide.",
			"Learn how to optimize your code for better performance and efficiency in just a few steps.",
			"Explore the benefits of using modern JavaScript frameworks in your projects and how they can enhance your development process.",
			"Tips and tricks for mastering CSS Grid and Flexbox layouts to create responsive and visually appealing websites.",
			"A beginner's guide to building responsive and user-friendly websites, covering everything from basic HTML to advanced CSS techniques.",
			"Unlock the secrets to creating dynamic and interactive web applications with our in-depth tutorial on JavaScript and AJAX.",
			"Improve your coding practices with these best practices and techniques, and take your development skills to the next level.",
			"Stay updated with the newest features and updates in .NET development, and learn how to leverage them in your projects.",
			"Enhance your productivity with these essential developer tools and resources, and streamline your workflow for maximum efficiency.",
			"Dive into the world of cloud computing and Azure, and learn how to leverage its power for your projects with our step-by-step guide."
		}.OrderBy(x => RandomNumberGenerator.GetInt32(int.MaxValue))
		.ToList();

		return blogDescriptions.First();
	}

	static List<Comment> GetComments()
	{
		var comments = new List<string>
		{
			"Great insights on mastering C#! This was really helpful.",
			"Python for data science is a game-changer. Thanks for the tips!",
			"Clean code is so important. Loved your tips!",
			"I recently started using C# for a project at work, and your post on mastering the basics was incredibly helpful. I was struggling with understanding some of the core concepts, but your explanations made everything so much clearer. I especially appreciated the section on object-oriented programming, as it really helped me grasp how to structure my code more effectively. Thanks for sharing such valuable insights!",
			"JavaScript web development is fascinating. Great read!",
			"AI and machine learning have such a bright future. Exciting times!",
			"Flutter makes mobile app development so much easier. Thanks for sharing!",
			"Version control with Git is a must-know. Great guide!",
			"Optimizing .NET applications is crucial. Thanks for the tips!",
			"Functional programming is a new concept for me. Thanks for the introduction!",
			"CSS Grid is a powerful tool. Loved the examples!",
			"Game development with Unity sounds fun. Great post!",
			"Secure coding practices are essential. Thanks for the best practices!",
			"Azure cloud computing is so powerful. Thanks for the guide!",
			"Debugging techniques are always useful. Great tips!",
			"Your post on optimizing .NET applications came at the perfect time for me. I've been working on a large-scale application, and performance has been a major concern. Your tips on minifying CSS and JavaScript files, using lazy loading for images, and optimizing server response times were exactly what I needed. After implementing these techniques, I noticed a significant improvement in the application's performance. Thank you for providing such practical and actionable advice!",
			"Azure cloud computing is the future. Thanks for the guide!",
			"I can't thank you enough for your detailed guide on building responsive websites with CSS Grid. As a front-end developer, creating responsive designs has always been a challenge for me. Your step-by-step instructions and example code made it so much easier to understand how to use CSS Grid effectively. I followed your guide to create a responsive layout for a client's website, and they were thrilled with the results. Your post has become my go-to resource for CSS Grid, and I recommend it to all my colleagues. Keep up the great work!"
		}
		.OrderBy(_ => RandomNumberGenerator.GetInt32(int.MaxValue))
		.Select(x => new Comment
		{
			Body = x,
			CreatedDate = DateTime.UtcNow.AddMinutes(RandomNumberGenerator.GetInt32(360, 1200)),
		})
		.Take(RandomNumberGenerator.GetInt32(1, 6))
		.ToList();

		return comments;
	}

	static string GetBody()
	{
		var postbodies = new List<string>
		{
			""""
        # Exploring the Latest Trends in Web Development

        ## Introduction
        In the ever-evolving world of web development, staying updated with the latest trends is crucial. This post delves into the most recent advancements and how they can enhance your projects.

        ## Optimizing Code for Performance
        Performance optimization is key to a successful web application. Learn how to streamline your code for better efficiency and faster load times.

        ### Key Techniques:
        - Minify CSS and JavaScript files
        - Use lazy loading for images
        - Optimize server response times
        ## Benefits of Modern JavaScript Frameworks
        Modern JavaScript frameworks like React, Angular, and Vue.js offer numerous benefits. Discover how they can simplify your development process and improve your application's performance.

        > "The right framework can significantly reduce development time and improve code maintainability." - [Web Dev Expert](https://example.com)

        ### Comparison Table:
        | Feature       | React | Angular | Vue.js |
        |---------------|-------|---------|--------|
        | Learning Curve| Medium| High    | Low    |
        | Performance   | High  | High    | High   |
        | Community Support | Excellent | Excellent | Good |
        ## Mastering CSS Grid and Flexbox
        CSS Grid and Flexbox are powerful tools for creating responsive layouts. This section provides tips and tricks for mastering these techniques to build visually appealing websites.

        ### Example Code:
        ```css
        .container {
          display: grid;
          grid-template-columns: repeat(3, 1fr);
          gap: 10px;
        }

        .item {
          background-color: #f0f0f0;
          padding: 20px;
        }

        ```
        ## Building Responsive Websites
        A beginner's guide to building responsive websites, covering everything from basic HTML to advanced CSS techniques. Learn how to create user-friendly and accessible web pages.

        ### Steps to Build a Responsive Website:
        1. Use a responsive grid system
        2. Implement media queries
        3. Optimize images for different screen sizes
        ## Creating Dynamic Web Applications
        Unlock the secrets to creating dynamic and interactive web applications with JavaScript and AJAX. This section offers an in-depth tutorial on building engaging user experiences.

        ### Sample AJAX Request:

        ```javascript
        fetch('https://api.example.com/data')
          .then(response => response.json())
          .then(data => console.log(data))
          .catch(error => console.error('Error:', error));
        ```

        ## Best Practices for Coding
        Improve your coding practices with these best practices and techniques. Enhance your development skills and write cleaner, more maintainable code.

        ### Best Practices:
        - Follow coding standards
        - Write unit tests
        - Use version control
        ## Staying Updated with .NET
        Stay updated with the newest features and updates in .NET development. Learn how to leverage these advancements in your projects to stay ahead of the curve.

        ### Latest .NET Features:
        - Improved performance
        - Enhanced security
        - New libraries and frameworks
        ## Enhancing Productivity with Developer Tools
        Discover essential developer tools and resources to enhance your productivity. Streamline your workflow and maximize efficiency with these tips.

        ### Recommended Tools:
        - Visual Studio Code
        - GitHub
        - Docker
        ## Leveraging Cloud Computing with Azure
        Dive into the world of cloud computing and Azure. Learn how to leverage its power for your projects with our step-by-step guide.

        ### Steps to Get Started with Azure:
        1. Create an Azure account
        2. Set up a resource group
        3. Deploy your first web app

        ## Conclusion
        Staying updated with the latest trends and best practices in web development is essential for success. By incorporating these techniques into your projects, you can create high-quality, efficient, and engaging web applications.

        ---

        ### Table of Contents
        1. [Introduction](#introduction)
        2. [Optimizing Code for Performance](#optimizing-code-for-performance)
        3. [Benefits of Modern JavaScript Frameworks](#benefits-of-modern-javascript-frameworks)
        4. [Mastering CSS Grid and Flexbox](#mastering-css-grid-and-flexbox)
        5. [Building Responsive Websites](#building-responsive-websites)
        6. [Creating Dynamic Web Applications](#creating-dynamic-web-applications)
        7. [Best Practices for Coding](#best-practices-for-coding)
        8. [Staying Updated with .NET](#staying-updated-with-net)
        9. [Enhancing Productivity with Developer Tools](#enhancing-productivity-with-developer-tools)
        10. [Leveraging Cloud Computing with Azure](#leveraging-cloud-computing-with-azure)
        11. [Conclusion](#conclusion)

        ---

        *Happy coding!*
        """",
			"""
      ## Getting Started with Blazor
      To get started with Blazor, you'll need to install the .NET SDK and create a new Blazor project.

      ### Installation Steps:
      1. Install the .NET SDK from the [official website](https://dotnet.microsoft.com/download).
      2. Create a new Blazor project using the following command:
          ```bash
          dotnet new blazorserver -o MyBlazorApp
          ```
      3. Navigate to the project directory and run the application:
          ```bash
          cd MyBlazorApp
          dotnet run
          ```
      ## Building Components in Blazor
      Blazor uses a component-based architecture, allowing you to build reusable UI components. Each component is a self-contained unit of UI and logic.

      ### Example Component:
      ```razor
      @code {
          private int count = 0;

          private void IncrementCount()
          {
              count++;
          }
      }

      <button @onclick="IncrementCount">Click me</button>
      <p>Current count: @count</p>
      ```
      ## Data Binding in Blazor
      Blazor supports two-way data binding, making it easy to synchronize data between the UI and the underlying data model.

      ### Example:
      ```
      @code {
          private string name;

          private void UpdateName(ChangeEventArgs e)
          {
              name = e.Value.ToString();
          }
      }

      <input @onchange="UpdateName" />
      <p>Hello, @name!</p>
      ```

      ## Handling Events in Blazor
      Blazor provides a simple and intuitive way to handle events, such as button clicks and form submissions.

      ### Example:
      ```razor
      @code {
          private void HandleSubmit()
          {
              // Handle form submission
          }
      }

      <form @onsubmit="HandleSubmit">
          <button type="submit">Submit</button>
      </form>
      ```

      ## Integrating with JavaScript
      Blazor allows you to call JavaScript functions from C# and vice versa, enabling seamless integration with existing JavaScript libraries.

      ### Example:
      ```razor
      @inject IJSRuntime JS

      @code {
          private async Task CallJavaScriptFunction()
          {
              await JS.InvokeVoidAsync("alert", "Hello from Blazor!");
          }
      }

      <button @onclick="CallJavaScriptFunction">Call JavaScript</button>
      ```
      ## Conclusion
      Blazor is a powerful framework that brings the power of .NET to web development. With its component-based architecture, seamless integration with JavaScript, and support for WebAssembly, Blazor is an excellent choice for building interactive web applications. By leveraging Blazor, you can create rich, dynamic web experiences using the familiar .NET ecosystem.

      ---

      ### Table of Contents
      1. [Introduction](#introduction)
      2. [What is Blazor?](#what-is-blazor)
      3. [Getting Started with Blazor](#getting-started-with-blazor)
      4. [Building Components in Blazor](#building-components-in-blazor)
      5. [Data Binding in Blazor](#data-binding-in-blazor)
      6. [Handling Events in Blazor](#handling-events-in-blazor)
      7. [Integrating with JavaScript](#integrating-with-javascript)
      8. [Conclusion](#conclusion)

      ---

      *Happy coding!*
      """
		}.OrderBy(x => RandomNumberGenerator.GetInt32(int.MaxValue))
		.ToList();

		return postbodies.First();
	}
}
