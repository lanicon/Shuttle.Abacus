namespace Shuttle.Abacus.UI.Core.Resources
{
    public class ClipboardResource
    {
        public Resource Resource { get; private set; }

        public ClipboardResource(Resource resource)
        {
            Resource = resource;
        }
    }
}