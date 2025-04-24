using System;
using System.Drawing;
using Newtonsoft.Json.Linq;
using Rhino;
using Rhino.DocObjects;
using Rhino.Geometry;
using rhinomcp.Serializers;
using Rhino.Runtime;
using System.Text;

namespace RhinoMCPPlugin.Functions;

public partial class RhinoMCPFunctions
{
    public JObject ExecuteRhinoscript(JObject parameters)
    {
        var doc = RhinoDoc.ActiveDoc;
        string code = parameters["code"]?.ToString();
        if (string.IsNullOrEmpty(code))
        {
            throw new Exception("Code is required");
        }

        try
        {
            var output = new StringBuilder();
            // Create a new Python script instance
            PythonScript pythonScript = PythonScript.Create();

            pythonScript.Output += (message) =>
            {
                output.Append(message);
            };

            // Setup the script context with the current document
            if (doc != null)
                pythonScript.SetupScriptContext(doc);

            // Execute the Python code
            pythonScript.ExecuteScript(code);


            return new JObject
            {
                ["success"] = true,
                ["result"] = $"Script successfully executed! Print output: {output}"
            };
        }
        catch (Exception ex)
        {
            return new JObject
            {
                ["success"] = false,
                ["message"] = $"Error executing rhinoscript: {ex.ToString()}"
            };
        }
    }
}