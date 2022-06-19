using System;
using System.Runtime.Serialization;
using UnityEngine;

namespace RPG.Saving
{
    [System.Serializable]
    public class SerializbleVector3 : Exception
    {
        private float X { get; }
        private float Y { get; }
        private float Z { get; }

        public SerializbleVector3(Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }
        protected SerializbleVector3(SerializationInfo info, StreamingContext context)
        {
            X = (float)info.GetValue("X", typeof(float));
            Y = (float)info.GetValue("Y", typeof(float));
            Z = (float)info.GetValue("Z", typeof(float));

        }
        public override void GetObjectData(
            SerializationInfo info, StreamingContext context)
        {
            if (info == null)
                throw new System.ArgumentNullException("info");
            info.AddValue("X", X, typeof(float));
            info.AddValue("Y", Y, typeof(float));
            info.AddValue("Z", Z, typeof(float));
        }
        public Vector3 ToVector3()
        {
            return new Vector3(this.X, this.Y, this.Z);
        }
        
        
    }
}