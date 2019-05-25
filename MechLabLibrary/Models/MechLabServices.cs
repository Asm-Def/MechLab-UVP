using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Threading;

namespace MechLabLibrary.Models
{
    public class MechLabServices
    {
        private readonly MechLabContext _context;
        private Mutex Working = new Mutex();
        public MechLabServices() =>  _context = new MechLabContext();
        public MechLabServices(MechLabContext context) => _context = context;

        /// <summary>
        /// 从数据库中读取所有场景的概要数据
        /// </summary>
        /// <returns></returns>
        public async Task<List<MechLabData>> GetMechLabs()
        {
            Working.WaitOne();
            List<MechLabData> res = await (new MechLabContext()).Labs.ToListAsync();
            Working.ReleaseMutex();
            return res;
        }

        ///// <summary>
        ///// 从数据库中读取编号为LabID的场景的所有Object
        ///// </summary>
        ///// <param name="LabID"></param>
        ///// <returns></returns>
        //public List<MechObject> GetMechLabObjects(Guid LabID)
        //{
        //    List<MechObjectData> objectDatas = _context.Objects.Where<MechObjectData>(o => o.LabID == LabID).ToList();
        //    List<MechObject> objects = new List<MechObject>();
        //    foreach (var obj in objectDatas)
        //    {
        //        if (obj.Type == "Planet")
        //        {
        //            objects.Add(new MechPlanet(objects.Count, obj.X, obj.Y, obj.VX, obj.VY, obj.M, obj.R == null ? 0 : (double)obj.R));

        //        }
        //        else // "Object"
        //        {
        //            objects.Add(new MechObject(objects.Count, obj.X, obj.Y, obj.VX, obj.VY, obj.M));
        //        }
        //    }
        //    return objects;
        //}

        public MechLabData GetLabData(Guid LabID)
        {
            Working.WaitOne();
            MechLabData res = (new MechLabContext()).Labs.FirstOrDefault(o => o.LabID == LabID);
            Working.ReleaseMutex();
            return res;
        }

        /// <summary>
        /// 从数据库中读取编号为LabID的Simulator数据
        /// </summary>
        /// <param name="LabID"></param>
        /// <returns></returns>
        public async Task<MechSimulator> GetSimulator(Guid LabID)
        {
            Working.WaitOne();
            IEnumerable<MechObjectData> objectDatas = null;
            using (var context = new MechLabContext())
            {
                objectDatas = await context.Objects.Where(o => o.LabID == LabID).ToListAsync();
            }
            Working.ReleaseMutex();
            List<MechObject> objects = new List<MechObject>();
            MechSimulator sim = new MechSimulator(LabID);
            foreach (var obj in objectDatas)
            {
                if (obj.Type == "Planet")
                {
                    sim.AddPlanet(obj.X, obj.Y, obj.VX, obj.VY, obj.M, obj.R);
                }
                else // "Object"
                {
                    sim.AddObject(obj.X, obj.Y, obj.VX, obj.VY, obj.M);
                }
            }
            return sim;
        }
        
        /// <summary>
        /// 保存实验场景（若不存在相应的LabID则新建）
        /// </summary>
        /// <param name="mechLabData"></param>
        /// <param name="objects"></param>
        public async Task SaveMechLab(MechLabData mechLabData, List<MechObject> objects)
        {
            var mechObjects = new List<MechObjectData>();
            var LabID = mechLabData.LabID;
            foreach(var obj in objects)
            {
                if (obj.IsPlanet)
                    mechObjects.Add(new MechObjectData { LabID = LabID, ObjectID = obj.ID, Type="Planet", X=obj.X, Y=obj.Y, VX=obj.VX, VY=obj.VY, M=obj.M, R = ((MechPlanet)obj).R });
                else
                    mechObjects.Add(new MechObjectData { LabID = LabID, ObjectID=obj.ID, Type="Object", X=obj.X, Y=obj.Y, VX=obj.VX, VY=obj.VY, M=obj.M });
            }

            Working.WaitOne();
            using (var context = new MechLabContext())
            {
                if (MechLabExists(mechLabData.LabID))
                {
                    Debug.WriteLine(mechLabData.LabID);
                    context.Labs.Update(mechLabData);

                    context.Objects.RemoveRange(context.Objects.Where<MechObjectData>(e => e.LabID == LabID));
                    await context.SaveChangesAsync();
                    //await _context.Entry(mechLabData).ReloadAsync();

                }
                else
                {
                    context.Labs.Add(mechLabData);
                    await context.SaveChangesAsync();
                }

                context.Objects.AddRange(mechObjects);
                await context.SaveChangesAsync();
            }
            Working.ReleaseMutex();
        }

        private bool MechLabExists(Guid ID) => (new MechLabContext()).Labs.Any(e => e.LabID == ID);
    }
}
