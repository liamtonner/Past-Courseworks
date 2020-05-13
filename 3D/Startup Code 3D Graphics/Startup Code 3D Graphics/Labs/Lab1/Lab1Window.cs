﻿using System;
using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;

namespace Labs.Lab1
{
    public class Lab1Window : GameWindow
    {
        private int[] mVertexBufferObjectIDArray = new int[2];
        private ShaderUtility mShader;

        public Lab1Window()
            : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "Lab 1 Hello, Triangle",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }

        protected override void OnLoad(EventArgs e)
        {
            GL.ClearColor(Color4.ForestGreen);

            float[] vertices = new float[] { 0.0f, 0.8f,
                                            0.8f, 0.4f,
                                            0.6f, -0.6f,
                                           -0.6f, -0.6f,
                                           -0.8f, 0.4f};

            uint[] indices = new uint[] { 4,3,0,
                                          2 ,
                                          1};

            GL.GenBuffers(2, mVertexBufferObjectIDArray);

            GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferObjectIDArray[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);

            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");

            }
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVertexBufferObjectIDArray[1]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);

            if (indices.Length * sizeof(uint) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }
            

            GL.Enable(EnableCap.CullFace);
            
            #region Shader Loading Code - Can be ignored for now

            mShader = new ShaderUtility( @"Lab1/Shaders/vSimple.vert", @"Lab1/Shaders/fSimple.frag");

            #endregion

            base.OnLoad(e);
        }

        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVertexBufferObjectIDArray[0]);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVertexBufferObjectIDArray[1]);

            // shader linking goes here
            #region Shader linking code - can be ignored for now

            GL.UseProgram(mShader.ShaderProgramID);
            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);

            #endregion

            GL.DrawElements(PrimitiveType.TriangleStrip, 5, DrawElementsType.UnsignedInt, 0);

            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            base.OnUnload(e);
            GL.DeleteBuffers(2, mVertexBufferObjectIDArray);
            GL.UseProgram(0);
            mShader.Delete();
        }
    }
}
//L1T1 Changed the clear colour and drew my first triangle!
//L1T2 Enabled Back Face Culling and Fixed Triangle Winding.
//L1T3 Drew a square by adding additional vertices to the vertices array, and adjusting the DrawArrays call.
//L1T4 This time only the second half of the square was drawn because it started drawing at array position 3 rather than 0
//L1T5 Drew a TriForceSymbol by editing the vertices array and adjusting the DrawArrays call. Also modified the fragment shader to colour all fragments yellow.
//L1T6 Converted TriForce Symbol to use element array buffers.
//L1T7 Drew a house by splitting it into 18 triangles, 28 vertices and 50 indices
//L1T8Drew a pentagon using DrawPrimitive.Triangles
//L1T9Drew a pentagon using DrawPrimitives.TriangleFan using 45% fewer indices!
//L1T10Drew a pentagon using DrawPrimitive.TriangleStrip

