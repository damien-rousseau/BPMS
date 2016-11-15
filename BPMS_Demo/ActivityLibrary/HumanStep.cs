using System.Activities;

namespace ActivityLibrary
{

    public sealed class HumanStep : NativeActivity
    {
        [RequiredArgument]
        public InArgument<string> StepName { get; set; }

        public OutArgument<bool> Decision { get; set; }

        protected override bool CanInduceIdle => true;


        protected override void Execute(NativeActivityContext context)
        {
            var bookmarkName = context.GetValue(StepName);

            // Create a bookmark allows the persistance in the DB
            context.CreateBookmark(bookmarkName, Continue);
        }

        private void Continue(NativeActivityContext context, Bookmark bookmark, object value)
        {
            if (value != null)
            {
                context.SetValue(Decision, (bool)value);
            }
        }
    }
}
