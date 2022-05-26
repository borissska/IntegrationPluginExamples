using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Alarus;

namespace FaceSelectionExample
{
	[PluginGUIName("Test face selection module")]
    [Guid("21338C27-6F5B-64B1-1E1C-4F66D68A4523")]
    internal sealed class PluginDefinition : IPlugin
    {
        public Guid Id => new Guid("21338C27-6F5B-64B1-1E1C-4F66D68A4523");

        public string Name => "Test face selection module";

        public string Manufacturer => "Example Company";

        public void Initialize(IPluginHost host)
        {
            List<Guid> guidList = new List<Guid>()
            {
                Guid.Parse("5C19153C-B100-431B-8164-CE8FD7E813FE")
            };
            host.RegisterRTVisualizer(typeof(FaceSelection), guidList);
        }
	}
}