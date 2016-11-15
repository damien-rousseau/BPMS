using System.Text;
using System.Activities;
using Common;
using Contracts;

namespace ActivityLibrary
{

    public sealed class SendEmail : CodeActivity
    {
        [RequiredArgument]
        public InArgument<string> From { get; set; }

        [RequiredArgument]
        public InArgument<string> To { get; set; }

        [RequiredArgument]
        public InArgument<string> Subject { get; set; }

        public InArgument<string> Link { get; set; }

        [RequiredArgument]
        public InArgument<string> Body { get; set; }

        protected override void Execute(CodeActivityContext context)
        {
            var body = context.GetValue(Body);
            var link = context.GetValue(Link);

            var sb = new StringBuilder();
            sb.AppendLine(body);

            if (!string.IsNullOrEmpty(link))
            {
                sb.AppendLine($"{link}?workflowId={context.WorkflowInstanceId}");
            }

            var emailService = IoC.Resolve<IEmailService>();
            emailService.SendEmail(context.GetValue(To), context.GetValue(Subject), sb.ToString(), context.GetValue(From));
        }
    }
}
