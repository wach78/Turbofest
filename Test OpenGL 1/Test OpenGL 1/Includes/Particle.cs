using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace OpenGL
{
    class Particle
    {
        Vector4[] colorSet; /*= {
          // Shades of red. 
          0.7f, 0.2f, 0.4f, 0.5f} , // rgba
          { 0.8f, 0.0f, 0.7f, 0.5f },
          { 1.0f, 0.0f, 0.0f, 0.5f },
          { 0.9f, 0.3f, 0.6f, 0.5f },
          { 1.0f, 0.4f, 0.0f, 0.5f },
          { 1.0f, 0.0f, 0.5f, 0.5f }
        ;*/
        int MaxColours;
        int Points;
        System.Collections.Generic.List<Vector3> m_vec3points;
        System.Collections.Generic.List<Vector2> m_vec2directions;
        System.Collections.Generic.List<Vector2> m_vec2velocity;
        int[] m_arrColour;
        float[] m_arrLifeOfPoint;
        float time;
        float deltatime;


        public Particle(int NumberOfPoints=50) 
        {
            this.m_vec3points = new List<Vector3>();
            this.m_vec2directions = new List<Vector2>();
            this.m_vec2velocity = new List<Vector2>();
            this.m_vec2velocity = new List<Vector2>();
            this.m_arrColour = new int[NumberOfPoints];
            this.m_arrLifeOfPoint = new float[NumberOfPoints];
            deltatime = 0.00025f;
            time = 0.0f;
            Random rnd = new Random();
            this.Points = NumberOfPoints;
            // Shades of red.
            colorSet = new Vector4[7];
            colorSet[0] = new Vector4(0.7f, 0.2f, 0.4f, 0.5f); // rgba
            colorSet[1] = new Vector4(0.8f, 0.0f, 0.7f, 0.5f);
            colorSet[2] = new Vector4(1.0f, 0.0f, 0.0f, 0.5f);
            colorSet[3] = new Vector4(0.9f, 0.3f, 0.6f, 0.5f);
            colorSet[4] = new Vector4(1.0f, 0.4f, 0.0f, 0.5f);
            colorSet[5] = new Vector4(1.0f, 0.0f, 0.5f, 0.5f);

            this.MaxColours = colorSet.Length / 4;
            
            

            float angle, velocity, direction;

            for (int i = 0; i < this.Points; i++)
            {
                m_vec3points.Add(new Vector3(0.0f, 0.0f, 0.0f));
                angle = rnd.Next(60, 70) * 1.0f;
                direction = rnd.Next(0, 360) * 1.0f;
                velocity = 3.0f * (rnd.Next(-8, 10) / 10.0f);
                m_vec2directions.Add(new Vector2(angle, direction));
                m_vec2velocity.Add(new Vector2((float) (velocity * Math.Cos(angle)), (float)(velocity * Math.Sin(angle))));
                this.m_arrColour[i] = rnd.Next() % this.MaxColours;
                this.m_arrLifeOfPoint[i] = 0.0f;
            }

            //System.Diagnostics.Debug.WriteLine("in particle constructor");
        }

        public void updateParticles()
        {
            float distance;
            for (int i = 0; i < this.Points; i++)
            {
                distance = this.m_vec2velocity[i].X * this.time;
                Vector3 v = m_vec3points[i];
                
                v.X = this.m_vec2directions[i].X * distance;
                v.Z = this.m_vec2directions[i].Y * distance;

                v.Y = (float) ((this.m_vec2velocity[i].Y - 0.5 * 2 * this.m_arrLifeOfPoint[i]) * this.m_arrLifeOfPoint[i]);

                if (v.Y <= 0.0f)
                {
                    if (distance > 12 /*fix this to a good size...*/)
                    {
                        this.m_arrColour[i] = this.MaxColours;
                        continue;
                    }
                    Vector2 vel = this.m_vec2velocity[i];
                    vel.Y *= 0.8f;
                    this.m_vec2velocity.RemoveAt(i);
                    this.m_vec2velocity.Insert(i, vel);
                    this.m_arrLifeOfPoint[i] = 0.0f;
                }
                this.m_arrLifeOfPoint[i] += deltatime;
                m_vec3points.RemoveAt(i);
                m_vec3points.Insert(i, v);

            }

            time += deltatime;
        }

        public void drawParticles()
        {
            updateParticles();

            GL.PointSize(5.0f);
            GL.PushAttrib(AttribMask.CurrentBit);
            GL.Begin(BeginMode.Points);
            for (int i = 0; i < this.Points; i++)
            {
                /* Draw alive particles. */
                /*if (colorList[i] != DEAD)
                {*/

                GL.Color4(colorSet[m_arrColour[i]]);
                
                GL.Vertex3(this.m_vec3points[i]);
                //}
            }
            GL.End();
            GL.PopAttrib();
        }

    }
}
