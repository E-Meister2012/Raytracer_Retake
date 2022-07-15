using OpenTK;
namespace Template
{
	class MyApplication
	{
		// member variables
		public Surface screen;
		Raytracer raytracer;
		internal static Camera camera;
		internal static Scene scene;
		// initialize
		public void Init()
		{
			camera = new Camera(new Vector3(0.6f, 0.4f, 1f), new Vector3(0, 0, 1), new Vector3(0, 1, 0));
			scene = new Scene();
			scene.primitives.Add(new Triangle(new Vector3(0, 0, 20), new Vector3(10, 0, 20), new Vector3(7, 8, 18), new Vector3(1)));
			scene.primitives.Add(new Triangle(new Vector3(0.4f, 0.3f, 1.1f), new Vector3(0.6f, 0.3f, 1.1f), new Vector3(0.5f, 0.6f, 1.1f), new Vector3(1, 1, 0)));
			scene.lights.Add(new Light(new Vector3(2, 2, 2f), 10f, new Vector3(1, 1, 1)));
			raytracer = new Raytracer(scene, camera);
		}
		// tick: renders one frame
		public void Tick()
		{
			screen.Clear( 0 );
			raytracer.Render();

		}
	}
}