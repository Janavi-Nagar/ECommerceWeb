using Microsoft.AspNetCore.Razor.Language;
using RazorEngine.Configuration;
using RazorEngine.Templating;

namespace ECommerceWeb
{
    public class HtmlTemplate
    {
        private static readonly IRazorEngineService RazorEngine;
        public object Model { get; set; }
        public string TemplateName { get; set; }

        public HtmlTemplate(string templateName)
        {
            TemplateName = templateName;            
        }

        public string GenerateBody()
        {
            var layout = RazorEngine.RunCompile("_Layout");
            var body = RazorEngine.RunCompile(TemplateName, Model.GetType(), Model);
            return layout.Replace("{{BODY}}", body);
        }

        static HtmlTemplate()
        {
            var config = new TemplateServiceConfiguration
            {
                TemplateManager = new EmbeddedTemplateManager(typeof(HtmlTemplate).Namespace + ".Templates"),
                Namespaces = { "ECommerceWeb", "ECommerceWeb.Models" },
                CachingProvider = new DefaultCachingProvider()
            };
            RazorEngine = RazorEngineService.Create(config);
        }
    }

    public class HtmlTemplate<TModel> : HtmlTemplate where TModel : class
    {
        public HtmlTemplate(string templateName, TModel mailModel) : base(templateName)
        {
            Model = mailModel;
        }
    }
}
