using Labs.Utility;
using OpenTK;
using OpenTK.Graphics;
using OpenTK.Graphics.OpenGL;
using System;
using System.Drawing;

namespace Labs.ACW
{
    public class ACWWindow : GameWindow
    {

        public ACWWindow()
           : base(
                800, // Width
                600, // Height
                GraphicsMode.Default,
                "552450 3D Graphics ACW Scene",
                GameWindowFlags.Default,
                DisplayDevice.Default,
                3, // major
                3, // minor
                GraphicsContextFlags.ForwardCompatible
                )
        {
        }

        private int[] mVBO_IDs = new int[7];
        private int[] mVAO_IDs = new int[4];
        private ShaderUtility mShader;
        private ShaderUtility Shader;
        private int mTexture_ID;
        private ModelUtility mCylinderModelUtility;
        private ModelUtility mMonsModelUtility;
        private Matrix4 mView, mCylinderModel, mGroundModel, mMonsModel, mCubeModel, mEye;

        protected override void OnLoad(EventArgs e)
        {
            // Set some GL state
            GL.ClearColor(Color4.MediumPurple);
            GL.Enable(EnableCap.DepthTest);
            //GL.Enable(EnableCap.CullFace);

            string filepath = @"Lab4/texture1.jpg";
            if (System.IO.File.Exists(filepath))
            {
                mShader = new ShaderUtility(@"Lab4/Shaders/vTexture.vert", @"Lab4/Shaders/fTexture.frag");
                GL.UseProgram(mShader.ShaderProgramID);

                Bitmap TextureBitmap = new Bitmap(filepath);
                TextureBitmap.RotateFlip(RotateFlipType.RotateNoneFlipY);
                System.Drawing.Imaging.BitmapData TextureData = TextureBitmap.LockBits(
                    new System.Drawing.Rectangle(0, 0, TextureBitmap.Width,
                TextureBitmap.Height), System.Drawing.Imaging.ImageLockMode.ReadOnly,
                System.Drawing.Imaging.PixelFormat.Format32bppRgb);

                GL.ActiveTexture(TextureUnit.Texture0);
                GL.GenTextures(1, out mTexture_ID);
                GL.BindTexture(TextureTarget.Texture2D, mTexture_ID);
                GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, TextureData.Width, TextureData.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, TextureData.Scan0);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
                GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);
                TextureBitmap.UnlockBits(TextureData);

                int uTextureSamplerLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uTextureSampler");
                GL.Uniform1(uTextureSamplerLocation, 0);

            }

            mShader = new ShaderUtility(@"ACW/Shaders/vPassThrough.vert", @"ACW/Shaders/fLighting.frag");
            GL.UseProgram(mShader.ShaderProgramID);
            int vPositionLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vPosition");
            int vNormalLocation = GL.GetAttribLocation(mShader.ShaderProgramID, "vNormal");

            //spotlight properties            //constant, linear, quadratic,

            int SpotLightPositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "light.Position");
            Vector4 SpotLightPosition = new Vector4(0, 4, 0f, 1);
            SpotLightPosition = Vector4.Transform(SpotLightPosition, mView);
            GL.Uniform4(SpotLightPositionLocation, SpotLightPosition);

            int SpotLightDirectionLoction = GL.GetUniformLocation(mShader.ShaderProgramID, "light.direction");
            Vector3 SpotLightDirection = new Vector3(0.0f, 3.0f, -5.0f);
            GL.Uniform3(SpotLightDirectionLoction, SpotLightDirection);

            int constantLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "light.constant");
            float constant = 1.0f;
            GL.Uniform1(constantLocation, constant);

            int linearLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "light.linear");
            float linear = 0.08f;
            GL.Uniform1(linearLocation, linear);

            int quadraticLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "light.quadratic");
            float quadratic = 0.025f;
            GL.Uniform1(quadraticLocation, quadratic);

            int AmbientSpotLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "light.AmbientLight");
            Vector3 SpotLightColour1 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(AmbientSpotLightLocation, SpotLightColour1);

            int DiffuseSpotLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "light.DiffuseLight");
            Vector3 SpotLightColour2 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(DiffuseSpotLightLocation, SpotLightColour2);

            int SpecularSpotLightLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "light.SpecularLight");
            Vector3 SpotLightColour3 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(SpecularSpotLightLocation, SpotLightColour3);

            //Light1 Properties (red)

            int uLight1PositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].Position");
            Vector4 light1Position = new Vector4(-2, 4, -9.5f, 1);
            light1Position = Vector4.Transform(light1Position, mView);
            GL.Uniform4(uLight1PositionLocation, light1Position);

            int uAmbientLight1Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].AmbientLight");
            Vector3 light1colour1 = new Vector3(0.1f, 0.0f, 0.0f);
            GL.Uniform3(uAmbientLight1Location, light1colour1);

            int uDiffuseLight1Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].DiffuseLight");
            Vector3 light1colour2 = new Vector3(1.0f, 0.0f, 0.0f);
            GL.Uniform3(uDiffuseLight1Location, light1colour2);

            int uSpecularLight1Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].SpecularLight");
            Vector3 light1colour3 = new Vector3(0.1f, 0.0f, 0.0f);
            GL.Uniform3(uSpecularLight1Location, light1colour3);

            //Light2 Properties (green)

            int uLight2PositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].Position");
            Vector4 light2Position = new Vector4(0, 4, -9.5f, 1);
            light2Position = Vector4.Transform(light2Position, mView);
            GL.Uniform4(uLight2PositionLocation, light2Position);

            int uAmbientLight2Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].AmbientLight");
            Vector3 light2colour1 = new Vector3(0.0f, 0.1f, 0.0f);
            GL.Uniform3(uAmbientLight2Location, light2colour1);

            int uDiffuseLight2Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].DiffuseLight");
            Vector3 light2colour2 = new Vector3(0.0f, 1.0f, 0.0f);
            GL.Uniform3(uDiffuseLight2Location, light2colour2);

            int uSpecularLight2Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].SpecularLight");
            Vector3 light2colour3 = new Vector3(0.0f, 0.1f, 0.0f);
            GL.Uniform3(uSpecularLight2Location, light2colour3);

            //Light3 Properties (blue)

            int uLight3PositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].Position");
            Vector4 light3Position = new Vector4(2, 4, -9.5f, 1);
            light3Position = Vector4.Transform(light3Position, mView);
            GL.Uniform4(uLight3PositionLocation, light3Position);

            int uAmbientLight3Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].AmbientLight");
            Vector3 light3colour1 = new Vector3(0.0f, 0.0f, 0.1f);
            GL.Uniform3(uAmbientLight2Location, light3colour1);

            int uDiffuseLight3Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].DiffuseLight");
            Vector3 light3colour2 = new Vector3(0.0f, 0.0f, 1.0f);
            GL.Uniform3(uDiffuseLight3Location, light3colour2);

            int uSpecularLight3Location = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].SpecularLight");
            Vector3 light3colour3 = new Vector3(0.0f, 0.0f, 0.1f);
            GL.Uniform3(uSpecularLight3Location, light3colour3);

            //Material Properties

            int uAmbientReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            Vector3 colour3 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(uAmbientReflectivityLocation, colour3);

            int uDiffuseReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            Vector3 colour4 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(uDiffuseReflectivityLocation, colour4);

            int uSpecularReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            Vector3 colour5 = new Vector3(1.0f, 1.0f, 1.0f);
            GL.Uniform3(uSpecularReflectivityLocation, colour5);

            int uShininessLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");
            float uShininess = 0.6f;
            GL.Uniform1(uShininessLocation, uShininess);

            GL.GenVertexArrays(mVAO_IDs.Length, mVAO_IDs);
            GL.GenBuffers(mVBO_IDs.Length, mVBO_IDs);
            //floor

            float[] vertices = new float[] {-10, 0, -10,0,1,0,
                                             -10, 0, 10,0,1,0,
                                             10, 0, 10,0,1,0,
                                             10, 0, -10,0,1,0,};

            GL.BindVertexArray(mVAO_IDs[0]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[0]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(vertices.Length * sizeof(float)), vertices, BufferUsageHint.StaticDraw);

            int size;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            //models

            mCylinderModelUtility = ModelUtility.LoadModel(@"Utility/Models/cylinder.bin");

            mMonsModelUtility = ModelUtility.LoadModel(@"Utility/Models/model.bin");


            //Cylinder
            GL.BindVertexArray(mVAO_IDs[1]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[1]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mCylinderModelUtility.Vertices.Length * sizeof(float)), mCylinderModelUtility.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[2]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mCylinderModelUtility.Indices.Length * sizeof(float)), mCylinderModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mCylinderModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

            //Monster
            GL.BindVertexArray(mVAO_IDs[2]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[3]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(mMonsModelUtility.Vertices.Length * sizeof(float)), mMonsModelUtility.Vertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[4]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(mMonsModelUtility.Indices.Length * sizeof(float)), mMonsModelUtility.Indices, BufferUsageHint.StaticDraw);

            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mMonsModelUtility.Vertices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out size);
            if (mMonsModelUtility.Indices.Length * sizeof(float) != size)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, true, 6 * sizeof(float), 3 * sizeof(float));

 


        //Cube
        float[] cVertices = new float[] {         //Tex coords
        -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f,
         1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f,
         1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,
         1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,
        -1.0f,  1.0f, -1.0f,  0.0f,  0.0f, -1.0f,
        -1.0f, -1.0f, -1.0f,  0.0f,  0.0f, -1.0f,

        -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f,
         1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f,
         1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f,
         1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f,
        -1.0f,  1.0f,  1.0f,  0.0f,  0.0f,  1.0f,
        -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,  1.0f,

        -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f,
        -1.0f,  1.0f, -1.0f, -1.0f,  0.0f,  0.0f,
        -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f,
        -1.0f, -1.0f, -1.0f, -1.0f,  0.0f,  0.0f,
        -1.0f, -1.0f,  1.0f, -1.0f,  0.0f,  0.0f,
        -1.0f,  1.0f,  1.0f, -1.0f,  0.0f,  0.0f,

         1.0f,  1.0f,  1.0f,  1.0f,  0.0f,  0.0f,
         1.0f,  1.0f, -1.0f,  1.0f,  0.0f,  0.0f,
         1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,
         1.0f, -1.0f, -1.0f,  1.0f,  0.0f,  0.0f,
         1.0f, -1.0f,  1.0f,  1.0f,  0.0f,  0.0f,
         1.0f,  1.0f,  1.0f,  1.0f,  0.0f,  0.0f,

        -1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,
         1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,
         1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,
         1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,
        -1.0f, -1.0f,  1.0f,  0.0f, -1.0f,  0.0f,
        -1.0f, -1.0f, -1.0f,  0.0f, -1.0f,  0.0f,

        -1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f,
         1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f,
         1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,
         1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,
        -1.0f,  1.0f,  1.0f,  0.0f,  1.0f,  0.0f,
        -1.0f,  1.0f, -1.0f,  0.0f,  1.0f,  0.0f};

            uint[] indices = {
                0,  1,  2,  3,  4,  5,   //front
                6,  7,  8,  9,  10,  11,   //right
                12,  13,  14, 15,  16, 17,  //back
                18, 19, 20, 21, 22, 23,  //left
                24, 25, 26, 27, 28, 29,  //upper
                30, 31, 32, 33, 34, 35}; //bottom };

            //GL.Enable(EnableCap.CullFace);
            GL.BindVertexArray(mVAO_IDs[3]);
            GL.BindBuffer(BufferTarget.ArrayBuffer, mVBO_IDs[5]);
            GL.BufferData(BufferTarget.ArrayBuffer, (IntPtr)(cVertices.Length * sizeof(float)), cVertices, BufferUsageHint.StaticDraw);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, mVBO_IDs[6]);
            GL.BufferData(BufferTarget.ElementArrayBuffer, (IntPtr)(indices.Length * sizeof(uint)), indices, BufferUsageHint.StaticDraw);

            int cSize;
            GL.GetBufferParameter(BufferTarget.ArrayBuffer, BufferParameterName.BufferSize, out cSize);
            if (cVertices.Length * sizeof(float) != cSize)
            {
                throw new ApplicationException("Vertex data not loaded onto graphics card correctly");
            }

            GL.GetBufferParameter(BufferTarget.ElementArrayBuffer, BufferParameterName.BufferSize, out cSize);
            if (indices.Length * sizeof(uint) != cSize)
            {
                throw new ApplicationException("Index data not loaded onto graphics card correctly");
            }

            GL.EnableVertexAttribArray(vPositionLocation);
            GL.VertexAttribPointer(vPositionLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 0);
            GL.EnableVertexAttribArray(vNormalLocation);
            GL.VertexAttribPointer(vNormalLocation, 3, VertexAttribPointerType.Float, false, 6 * sizeof(float), 3 * sizeof(float));


            GL.BindVertexArray(0);

            mView = Matrix4.CreateTranslation(0, -3f, 0);
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);


            mEye = Matrix4.CreateTranslation(0, -1.5f, 0);
            int uEye = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.UniformMatrix4(uEye, true, ref mEye);

            mGroundModel = Matrix4.CreateTranslation(0, 0, -5f);
            mCylinderModel = Matrix4.CreateTranslation(0, 1, -5f);
            mMonsModel = Matrix4.CreateTranslation(0, 3, -5f);
            mCubeModel = Matrix4.CreateTranslation(5, 1, -3);

            base.OnLoad(e);

        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(this.ClientRectangle);
            if (mShader != null)
            {
                int uProjectionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uProjection");
                Matrix4 projection = Matrix4.CreatePerspectiveFieldOfView(1, (float)ClientRectangle.Width / ClientRectangle.Height, 0.5f, 25);
                GL.UniformMatrix4(uProjectionLocation, true, ref projection);
            }
        }

        private void lightPos()
        {
            int uLight1PositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[0].Position");
            Vector4 light1Position = new Vector4(-5, 6, -9.5f, 1);
            light1Position = Vector4.Transform(light1Position, mView);
            GL.Uniform4(uLight1PositionLocation, light1Position);

            int uLight2PositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[1].Position");
            Vector4 light2Position = new Vector4(0, 6, -9.5f, 1);
            light2Position = Vector4.Transform(light2Position, mView);
            GL.Uniform4(uLight2PositionLocation, light2Position);

            int uLight3PositionLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uLight[2].Position");
            Vector4 light3Position = new Vector4(5, 6, -9.5f, 1);
            light3Position = Vector4.Transform(light3Position, mView);
            GL.Uniform4(uLight3PositionLocation, light3Position);
        }

        protected override void OnKeyPress(KeyPressEventArgs e)
        {

            base.OnKeyPress(e);
            if (e.KeyChar == 'w')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, 0.15f);
                mEye = mEye * Matrix4.CreateTranslation(0.0f, 0.0f, 0.15f);
                MoveCamera();
            }
            if (e.KeyChar == 's')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, 0.0f, -0.15f);
                mEye = mEye * Matrix4.CreateTranslation(0.0f, 0.0f, -0.15f);
                MoveCamera();
            }
            if (e.KeyChar == 'a')
            {
                lightPos();
                mView = mView * Matrix4.CreateRotationY(-0.025f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);
            }
            if (e.KeyChar == 'd')
            {
                lightPos();
                mView = mView * Matrix4.CreateRotationY(0.025f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);

            }
            if (e.KeyChar == 'r')
            {
                lightPos();
                mView = mView * Matrix4.CreateRotationX(-0.025f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);

            }
            if (e.KeyChar == 'f')
            {
                lightPos();
                mView = mView * Matrix4.CreateRotationX(0.025f);
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);

            }
            if (e.KeyChar == 'q')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, 0.5f, 0f);
                mEye = mEye * Matrix4.CreateTranslation(0.0f, 0.5f, 0f);
                MoveCamera();
            }
            if (e.KeyChar == 'e')
            {
                mView = mView * Matrix4.CreateTranslation(0.0f, -0.5f, 0f);
                mEye = mEye * Matrix4.CreateTranslation(0.0f, -0.5f, 0f);
                MoveCamera();
            }
            if (e.KeyChar == 'c')
            {
                Vector3 t = mMonsModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mMonsModel = mMonsModel * inverseTranslation * Matrix4.CreateRotationY(-0.025f) * translation;
            }
            if (e.KeyChar == 'v')
            {
                Vector3 t = mMonsModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mMonsModel = mMonsModel * inverseTranslation * Matrix4.CreateRotationY(0.025f) * translation;
            }
            if (e.KeyChar == 'z')
            {
                Vector3 t = mGroundModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mGroundModel = mGroundModel * inverseTranslation * Matrix4.CreateRotationY(-0.050f) * translation;
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);
            }
            if (e.KeyChar == 'x')
            {
                Vector3 t = mGroundModel.ExtractTranslation();
                Matrix4 translation = Matrix4.CreateTranslation(t);
                Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
                mGroundModel = mGroundModel * inverseTranslation * Matrix4.CreateRotationY(0.050f) * translation;
                int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
                GL.UniformMatrix4(uView, true, ref mView);

            }
        }


        private void MoveCamera()
        {
            int uView = GL.GetUniformLocation(mShader.ShaderProgramID, "uView");
            GL.UniformMatrix4(uView, true, ref mView);

            int uEye = GL.GetUniformLocation(mShader.ShaderProgramID, "uEyePosition");
            GL.UniformMatrix4(uEye, true, ref mView);

            lightPos();
        }
        private void Mon()
        {
            int uAmbientReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            Vector3 monColour3 = new Vector3(0.25f, 0.25f, 0.25f);
            GL.Uniform3(uAmbientReflectivityLocation, monColour3);

            int uDiffuseReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            Vector3 monColour4 = new Vector3(0.4f, 0.4f, 0.4f);
            GL.Uniform3(uDiffuseReflectivityLocation, monColour4);

            int uSpecularReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            Vector3 monColour5 = new Vector3(0.774597f, 0.774597f, 0.774597f);
            GL.Uniform3(uSpecularReflectivityLocation, monColour5);

            int uShininessLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");
            float monUShininess = 0.6f;
            GL.Uniform1(uShininessLocation, monUShininess);
        }

        private void Cyl()
        {
            int uAmbientReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            Vector3 colour3 = new Vector3(0.19225f, 0.19225f, 0.19225f);
            GL.Uniform3(uAmbientReflectivityLocation, colour3);

            int uDiffuseReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            Vector3 colour4 = new Vector3(0.50754f, 0.50754f, 0.50754f);
            GL.Uniform3(uDiffuseReflectivityLocation, colour4);

            int uSpecularReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            Vector3 colour5 = new Vector3(0.508273f, 0.508273f, 0.508273f);
            GL.Uniform3(uSpecularReflectivityLocation, colour5);

            int uShininessLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");
            float uShininess = 0.4f;
            GL.Uniform1(uShininessLocation, uShininess);

        }

        private void Floor()
        {
            int uAmbientReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            Vector3 colour3 = new Vector3(0.05f, 0.05f, 0.05f);
            GL.Uniform3(uAmbientReflectivityLocation, colour3);

            int uDiffuseReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            Vector3 colour4 = new Vector3(0.5f, 0.5f, 0.5f);
            GL.Uniform3(uDiffuseReflectivityLocation, colour4);

            int uSpecularReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            Vector3 colour5 = new Vector3(0.7f, 0.7f, 0.7f);
            GL.Uniform3(uSpecularReflectivityLocation, colour5);

            int uShininessLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");
            float uShininess = 0.078125f;
            GL.Uniform1(uShininessLocation, uShininess);

        }

        private void Cube()
        {
            int uAmbientReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.AmbientReflectivity");
            Vector3 colour3 = new Vector3(0.1745f, 0.01175f, 0.01175f);
            GL.Uniform3(uAmbientReflectivityLocation, colour3);

            int uDiffuseReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.DiffuseReflectivity");
            Vector3 colour4 = new Vector3(0.61424f, 0.04136f, 0.04136f);
            GL.Uniform3(uDiffuseReflectivityLocation, colour4);

            int uSpecularReflectivityLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.SpecularReflectivity");
            Vector3 colour5 = new Vector3(0.727811f, 0.626959f, 0.626959f);
            GL.Uniform3(uSpecularReflectivityLocation, colour5);

            int uShininessLocation = GL.GetUniformLocation(mShader.ShaderProgramID, "uMaterial.Shininess");
            float uShininess = 0.6f;
            GL.Uniform1(uShininessLocation, uShininess);

        }



        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);
            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);


            //floor
            int uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref mGroundModel);

            Floor();

            GL.BindVertexArray(mVAO_IDs[0]);
            GL.DrawArrays(PrimitiveType.TriangleFan, 0, 4);

            //cylinder
            Matrix4 m = mCylinderModel * mGroundModel;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref m);

            Cyl();

            GL.BindVertexArray(mVAO_IDs[1]);
            GL.DrawElements(PrimitiveType.Triangles, mCylinderModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            //monster
            Matrix4 p = mMonsModel * mGroundModel;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref p);

            Mon();

            Vector3 t = mMonsModel.ExtractTranslation();
            Matrix4 translation = Matrix4.CreateTranslation(t);
            Matrix4 inverseTranslation = Matrix4.CreateTranslation(-t);
            mMonsModel = mMonsModel * inverseTranslation * Matrix4.CreateRotationY(-0.025f) * translation;


            GL.BindVertexArray(mVAO_IDs[2]);
            GL.DrawElements(PrimitiveType.Triangles, mMonsModelUtility.Indices.Length, DrawElementsType.UnsignedInt, 0);

            //Cube
            Matrix4 T = mCubeModel * mGroundModel;
            uModel = GL.GetUniformLocation(mShader.ShaderProgramID, "uModel");
            GL.UniformMatrix4(uModel, true, ref T);

            Cube();
            GL.BindVertexArray(mVAO_IDs[3]);
            GL.DrawArrays(PrimitiveType.Triangles, 0, 36);




            GL.BindVertexArray(0);
            this.SwapBuffers();
        }

        protected override void OnUnload(EventArgs e)
        {
            GL.BindBuffer(BufferTarget.ArrayBuffer, 0);
            GL.BindBuffer(BufferTarget.ElementArrayBuffer, 0);
            GL.BindVertexArray(0);
            GL.DeleteBuffers(mVBO_IDs.Length, mVBO_IDs);
            GL.DeleteVertexArrays(mVAO_IDs.Length, mVAO_IDs);
            mShader.Delete();
            base.OnUnload(e);
        }
    }
}
