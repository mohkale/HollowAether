using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HollowAether.Lib.GAssets.FrameCounter {
	/*public class FrameCounter {
		public void Update(float deltaTime) {
			CurrentFramesPerSecond = 1.0f / deltaTime;
			FPSStorageBuffer.Enqueue(CurrentFramesPerSecond);

			if (FPSStorageBuffer.Count > MAX_BUFFER_STORE) {

			} else {

			}
		}

		/*public void bool Update(float deltaTime) {
			CurrentFramesPerSecond = 1.0f / deltaTime;

			_sampleBuffer.Enqueue(CurrentFramesPerSecond);

			if (_sampleBuffer.Count > MAXIMUM_SAMPLES) {
				_sampleBuffer.Dequeue();
				AverageFramesPerSecond = _sampleBuffer.Average(i => i);
			} else {
				AverageFramesPerSecond = CurrentFramesPerSecond;
			}

			TotalFrames++;
			TotalSeconds += deltaTime;
			return true;
		}

		public const int MAXIMUM_SAMPLES = 100;

		private Queue<float> _sampleBuffer = new Queue<float>();


		public long TotalFrames { get; private set; }
		public float TotalSeconds { get; private set; }
		public float AverageFramesPerSecond { get; private set; }
		public float CurrentFramesPerSecond { get; private set; }

		public const int MAX_BUFFER_STORE = 100;

		private Queue<float> FPSStorageBuffer = new Queue<float>();
	}*/
}
