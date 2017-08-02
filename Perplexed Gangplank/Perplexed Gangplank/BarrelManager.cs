﻿using Aimtec;
using Aimtec.SDK.Extensions;
using Aimtec.SDK.Prediction.Skillshots;
using System.Collections.Generic;
using System.Linq;
using Aimtec.SDK.Util.Cache;

namespace Perplexed_Gangplank
{
    public static class BarrelManager
    {
        public static List<Barrel> Barrels;
        public static bool BarrelWillHit(Barrel barrel, Obj_AI_Base target)
        {
            return barrel.Object.Distance(target) <= SpellManager.ExplosionRadius;
        }
        public static List<Obj_AI_Hero> GetEnemiesInChainRadius(Barrel barrel)
        {
            //Returns enemies within the barrel's chain radius, but outside of its explosion radius.
            return GameObjects.EnemyHeroes.Where(x => x.IsValidTarget() && barrel.Object.Distance(x) <= SpellManager.ChainRadius && barrel.Object.Distance(x) >= SpellManager.ExplosionRadius).ToList();
        }
        public static List<Barrel> GetBarrelsThatWillHit()
        {
            //Returns all barrels that can explode on an enemy.
            var barrelsWillHit = new List<Barrel>();
            foreach(var enemy in Utility.GetAllEnemiesInRange(float.MaxValue))
                barrelsWillHit.AddRange(GetBarrelsThatWillHit(enemy));
            return barrelsWillHit.Distinct().OrderBy(x => x.Object.Distance(Utility.Player)).ToList();
        }
        public static List<Barrel> GetBarrelsThatWillHit(Obj_AI_Base target)
        {
            return Barrels.Where(x => target.Distance(x.Object) <= SpellManager.ExplosionRadius).OrderBy(x => x.Object.Distance(Utility.Player)).ToList();
        }
        public static List<Barrel> GetChainedBarrels(Barrel barrel)
        {
            return Barrels.Where(x => !x.Object.IsDead && x.Object.Distance(barrel.Object) <= SpellManager.ChainRadius).ToList();
        }
        public static Barrel GetBestBarrelToQ(List<Barrel> barrels)
        {
            return barrels.Where(x => !x.Object.IsDead && x.CanQ && x.Object.IsInRange(SpellManager.Q.Range)).OrderBy(x => x.Object.Distance(Utility.Player)).FirstOrDefault();
        }
        public static Barrel GetNearestBarrel()
        {
            return Barrels.Where(x => !x.Object.IsDead).OrderBy(x => x.Object.Distance(Utility.Player)).FirstOrDefault();
        }
        public static Barrel GetNearestBarrel(Vector3 position)
        {
            return Barrels.Where(x => !x.Object.IsDead).OrderBy(x => x.Object.Distance(position)).FirstOrDefault();
        }
    }
}
