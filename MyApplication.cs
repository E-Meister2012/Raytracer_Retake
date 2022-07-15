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
			scene = new Scene();
			scene.primitives.Add(new Triangle(new Vector3(0, 1, 0), new Vector3(1, 0, 0), new Vector3(0, 0, 1), new Vector3(1)));
			scene.primitives.Add(new Triangle(new Vector3(0, 1, 5), new Vector3(3, 2, 0), new Vector3(4, 0, 2), new Vector3(1)));
			scene.lights.Add(new Light(new Vector3(6, 2, 10), new Vector3(1), new Vector3(1)));
			camera = new Camera(new Vector3(0,0,10), new Vector3(0, 0, -1), new Vector3(0, 1, 0), 100);
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