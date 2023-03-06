using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Xml.Serialization;
using LOR_XML;
using Mod;
using UnityEngine;

namespace SephirahTeams.Util
{
    public static class LocalizeUtil
    {
        public static void RemoveError()
        {
            Singleton<ModContentManager>.Instance.GetErrorLogs().RemoveAll(x => new List<string>
            {
                "0Harmony",
                "Mono.Cecil",
                "MonoMod.RuntimeDetour",
                "MonoMod.Utils"
            }.Exists(y => x.Contains("The same assembly name already exists. : " + y)));
        }

        public static void AddGlobalLocalize()
        {
            var error = false;
            FileInfo file;
            try
            {
                file = new DirectoryInfo(ModParameters.Path + "/Localize/" + ModParameters.Language + "/PassiveDesc")
                    .GetFiles().FirstOrDefault();
                error = true;
                if (file == null) return;
                using (var stringReader7 = new StringReader(File.ReadAllText(file.FullName)))
                {
                    var passiveDescRoot =
                        (PassiveDescRoot)new XmlSerializer(typeof(PassiveDescRoot)).Deserialize(stringReader7);
                    using (var enumerator9 = Singleton<PassiveXmlList>.Instance.GetDataAll()
                               .FindAll(x => x.id.packageId == ModParameters.PackageId).GetEnumerator())
                    {
                        while (enumerator9.MoveNext())
                        {
                            var passive = enumerator9.Current;
                            passive.name = passiveDescRoot.descList.Find(x => x.ID == passive.id.id).name;
                            passive.desc = passiveDescRoot.descList.Find(x => x.ID == passive.id.id).desc;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                if (error)
                    Debug.LogError("Error loading Passive Desc Texts packageId : " + ModParameters.PackageId +
                                   " Language : " +
                                   ModParameters.Language + " Error : " + ex.Message);
            }
        }
    }
}