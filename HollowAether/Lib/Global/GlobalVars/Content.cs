#define DebugOutputLoadImage

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Drawing;

#region XNAImports
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Audio;
#endregion

using IOMan = HollowAether.Lib.InputOutput.InputOutputManager;
using GV = HollowAether.Lib.GlobalVars;
using SUM = HollowAether.StartUpMethods;
using HollowAether.Lib.GAssets;

using System;
using System.Windows.Forms;
using System.Threading;
using Microsoft.Xna.Framework.Graphics;


namespace HollowAether.Lib {
	public static partial class GlobalVars {
		public static class Content {
			#region APIWrapperClasses
			/// <summary>
			/// Container class implements the IServiceProvider interface. This is used
			/// to pass shared services between different components, for instance the
			/// ContentManager uses it to locate the IGraphicsDeviceService implementation.
			/// </summary>
			public class ServiceContainer : IServiceProvider {
				/// <summary> Adds a new service to the collection.  </summary>
				public void AddService<T>(T service) {
					services.Add(typeof(T), service);
				}


				/// <summary> Looks up the specified service.</summary>
				public object GetService(Type serviceType) {
					object service;

					services.TryGetValue(serviceType, out service);

					return service;
				}

				Dictionary<Type, object> services = new Dictionary<Type, object>();
			}

			/// <summary> Helper class responsible for creating and managing the GraphicsDevice.
			/// All GraphicsDeviceControl instances share the same GraphicsDeviceService,
			/// so even though there can be many controls, there will only ever be a single
			/// underlying GraphicsDevice. This implements the standard IGraphicsDeviceService
			/// interface, which provides notification events for when the device is reset
			/// or disposed. </summary>
			class GraphicsDeviceService : IGraphicsDeviceService {
				#region Fields


				// Singleton device service instance.
				static GraphicsDeviceService singletonInstance;


				// Keep track of how many controls are sharing the singletonInstance.
				static int referenceCount;


				#endregion


				/// <summary> Constructor is private, because this is a singleton class:
				/// client controls should use the public AddRef method instead. </summary>
				GraphicsDeviceService(IntPtr windowHandle, int width, int height) {
					parameters = new PresentationParameters();

					parameters.BackBufferWidth = Math.Max(width, 1);
					parameters.BackBufferHeight = Math.Max(height, 1);
					parameters.BackBufferFormat = SurfaceFormat.Color;
					parameters.DepthStencilFormat = DepthFormat.Depth24;
					parameters.DeviceWindowHandle = windowHandle;
					parameters.PresentationInterval = PresentInterval.Immediate;
					parameters.IsFullScreen = false;

					graphicsDevice = new GraphicsDevice(GraphicsAdapter.DefaultAdapter,
														GraphicsProfile.Reach,
														parameters);
				}


				/// <summary> Gets a reference to the singleton instance. </summary>
				public static GraphicsDeviceService AddRef(IntPtr windowHandle,
														   int width, int height) {
					// Increment the "how many controls sharing the device" reference count.
					if (Interlocked.Increment(ref referenceCount) == 1) {
						// If this is the first control to start using the
						// device, we must create the singleton instance.
						singletonInstance = new GraphicsDeviceService(windowHandle,
																	  width, height);
					}

					return singletonInstance;
				}


				/// <summary> Releases a reference to the singleton instance. </summary>
				public void Release(bool disposing) {
					// Decrement the "how many controls sharing the device" reference count.
					if (Interlocked.Decrement(ref referenceCount) == 0) {
						// If this is the last control to finish using the
						// device, we should dispose the singleton instance.
						if (disposing) {
							if (DeviceDisposing != null)
								DeviceDisposing(this, EventArgs.Empty);

							graphicsDevice.Dispose();
						}

						graphicsDevice = null;
					}
				}


				/// <summary> Resets the graphics device to whichever is bigger out of the specified
				/// resolution or its current size. This behavior means the device will
				/// demand-grow to the largest of all its GraphicsDeviceControl clients. </summary>
				public void ResetDevice(int width, int height) {
					if (DeviceResetting != null)
						DeviceResetting(this, EventArgs.Empty);

					parameters.BackBufferWidth = Math.Max(parameters.BackBufferWidth, width);
					parameters.BackBufferHeight = Math.Max(parameters.BackBufferHeight, height);

					graphicsDevice.Reset(parameters);

					if (DeviceReset != null)
						DeviceReset(this, EventArgs.Empty);
				}


				/// <summary> Gets the current graphics device. </summary>
				public GraphicsDevice GraphicsDevice {
					get { return graphicsDevice; }
				}

				GraphicsDevice graphicsDevice;


				// Store the current device settings.
				PresentationParameters parameters;

				// IGraphicsDeviceService events.
				public event EventHandler<EventArgs> DeviceCreated;
				public event EventHandler<EventArgs> DeviceDisposing;
				public event EventHandler<EventArgs> DeviceReset;
				public event EventHandler<EventArgs> DeviceResetting;
			}
			#endregion

			public static void LoadContent() {
				ContentManager Content = GlobalVars.hollowAether.Content;

				Func<String, String> GetAllButFirstDirectory = (path) => {
					String[] splitPath = path.Split('\\'); // Split path into all sub paths including directories and files
					return GV.CollectionManipulator.GetSubArray(splitPath, 1, splitPath.Length - 1).Aggregate((a, b) => $"{a}\\{b}");
				};

				foreach (string contentPath in SUM.SystemParserTruncatedScriptInterpreter(Content.RootDirectory)) {
					String trueContentPath = contentPath.Replace(IOMan.GetExtension(contentPath), ""), // No Extension
						   key = GetAllButFirstDirectory(contentPath).Replace(IOMan.GetExtension(contentPath), "").ToLower();

					switch (IOMan.GetDirectoryName(contentPath).Split('\\')[0].ToLower()) {
						case "textures":
							MonoGameImplement.textures.Add(key, Content.Load<Texture2D>(trueContentPath));
							break;
						case "fonts":
							MonoGameImplement.fonts.Add(key, Content.Load<SpriteFont>(trueContentPath));
							break;
						case "video":
							MonoGameImplement.videos.Add(key, Content.Load<Video>(trueContentPath));
							break;
						case "sound":
							MonoGameImplement.sounds.Add(key, Content.Load<SoundEffect>(trueContentPath));
							break;
					}
				}
			}

			private static IEnumerable<String> GetAllTexturePaths() {
				Func<String, String> GetAllButFirstDirectory = (path) => {
					String[] splitPath = path.Split('\\'); // Split path into all sub paths including directories and files
					return GV.CollectionManipulator.GetSubArray(splitPath, 1, splitPath.Length - 1).Aggregate((a, b) => $"{a}\\{b}");
				};

				String rootDirectory = GV.hollowAether.Content.RootDirectory;
				
				foreach (string contentPath in SUM.SystemParserTruncatedScriptInterpreter(rootDirectory)) {
					//String trueContentPath = contentPath.Replace(IOMan.GetExtension(contentPath), ""); // No Extension
					//String key = GetAllButFirstDirectory(contentPath).Replace(IOMan.GetExtension(contentPath), "").ToLower();

					switch (IOMan.GetDirectoryName(contentPath).Split('\\')[0].ToLower()) {
						case "textures": yield return contentPath; break;
					}
				}
			}

			private static ContentManager GenerateContentManager() {
				Form form = new Form(); // Dummy form for creating a graphics device
				GraphicsDeviceService gds = GraphicsDeviceService.AddRef(form.Handle,
						form.ClientSize.Width, form.ClientSize.Height);

				ServiceContainer services = new ServiceContainer();
				services.AddService<IGraphicsDeviceService>(gds);

				return new ContentManager(services, GV.hollowAether.Content.RootDirectory);
			}

			public static IEnumerable<Tuple<Texture2D, String>> LoadTextures() {
				ContentManager manager = GenerateContentManager(); // Generate new content manager

				#if DEBUG && DebugOutputLoadImage
				Console.WriteLine("\tContent Manager Made\n");
				#endif

				foreach (String texturePath in GetAllTexturePaths()) {
					String withoutExtension = texturePath.Replace(IOMan.GetExtension(texturePath), "");

					#if DEBUG && DebugOutputLoadImage
					Console.WriteLine($"\t\tLoaded Texture '{texturePath}'");
					#endif

					yield return new Tuple<Texture2D, String>(manager.Load<Texture2D>(withoutExtension), texturePath);
				}

				manager.Dispose();

				#if DEBUG && DebugOutputLoadImage
				Console.WriteLine("\tContent Manager Disposed");
				#endif
			}

			public static Tuple<Image, String>[] LoadImages() {
				#if DEBUG && DebugOutputLoadImage
				Console.WriteLine("Image Load Begun");
				#endif

				List<Tuple<Image, String>> images = new List<Tuple<Image, String>>();

				foreach (Tuple<Texture2D, String> keyText in LoadTextures()) {
					Texture2D texture = keyText.Item1; // Alias for texture

					using (MemoryStream memoryStrem = new MemoryStream()) {
						texture.SaveAsPng(memoryStrem, texture.Width, texture.Height);
						Image image = Image.FromStream(memoryStrem); // Texture2D
						images.Add(new Tuple<Image, String>(image, keyText.Item2));
					}

					#if DEBUG && DebugOutputLoadImage
					Console.WriteLine($"\t\tCasted Texture To Image\n");
					#endif
				}

				#if DEBUG && DebugOutputLoadImage
				Console.WriteLine("Image Load Complete\n");
				#endif

				return images.ToArray();
			}
		}
	}
}
