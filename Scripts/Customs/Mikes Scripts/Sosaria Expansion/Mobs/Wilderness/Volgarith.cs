using System;
using System.Collections.Generic;
using System.Linq;                   // ← Needed for ToList()
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;               // SpellHelper, AOS.Damage, etc.

namespace Server.Mobiles
{
    [CorpseName("a volgarith corpse")]
    public class Volgarith : BaseCreature
    {
        private const int UniqueHue = 1175;

        [Constructable]
        public Volgarith()
            : base(AIType.AI_Melee, FightMode.Aggressor, 10, 1, 0.2, 0.4)
        {
            Name = "Volgarith";
            Body = 103;
            BaseSoundID = 362;
            Hue = UniqueHue;

            SetStr(8000, 8500);
            SetDex(500, 600);
            SetInt(1500, 1600);

            SetHits(25000, 30000);
            SetDamage(60, 100);

            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Cold, 20);
            SetDamageType(ResistanceType.Energy, 20);

            SetResistance(ResistanceType.Physical, 80, 85);
            SetResistance(ResistanceType.Fire, 75, 80);
            SetResistance(ResistanceType.Cold, 75, 80);
            SetResistance(ResistanceType.Poison, 100);
            SetResistance(ResistanceType.Energy, 75, 80);

            SetSkill(SkillName.Tactics, 120.0);
            SetSkill(SkillName.Wrestling, 120.0);
            SetSkill(SkillName.Magery, 120.0);
            SetSkill(SkillName.EvalInt, 120.0);
            SetSkill(SkillName.MagicResist, 120.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 80;
            CantWalk = false;

            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems);
        }

        public Volgarith(Serial serial)
            : base(serial)
        {
        }

        public override int GetIdleSound()   { return 0x2C4; }
        public override int GetAttackSound() { return 0x2C0; }
        public override int GetDeathSound()  { return 0x2C1; }
        public override int GetAngerSound()  { return 0x2C4; }
        public override int GetHurtSound()   { return 0x2C3; }

        private DateTime _NextAoEWave, _NextManaDrain, _NextTerror;

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            var target = Combatant as Mobile;
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 20) || !CanBeHarmful(target) || !InLOS(target))
                return;

            // ——— AOE Wave ———
            if (DateTime.UtcNow >= _NextAoEWave)
            {
                Animate(3, 5, 1, true, false, 0);
                Say("Feel my wrath!");

                Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
                {
                    if (target != null && !target.Deleted && target.Map == Map && InRange(target, 20) && CanBeHarmful(target) && InLOS(target))
                    {
                        DoHarmful(target);

                        int aoeRange = 8;
                        var list = AcquireIndirectTargets(target.Location, aoeRange).ToList();  // Now works

                        foreach (var mob in list)
                        {
                            DoHarmful(mob);
                            int damage = Utility.RandomMinMax(50, 80) + (Int / 20);
                            AOS.Damage(mob, this, damage, 40, 20, 20, 20, 0);
                            mob.PlaySound(0x1D1);
                            mob.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);
                        }

                        target.PlaySound(0x207);
                        target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                    }
                });

                _NextAoEWave = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }

            // ——— Mana Drain ———
            if (DateTime.UtcNow >= _NextManaDrain)
            {
                var list = AcquireIndirectTargets(Location, 6).ToList();

                foreach (var mob in list)
                {
                    DoHarmful(mob);
                    int manaDrain = Utility.RandomMinMax(15, 30);
                    mob.Mana = Math.Max(0, mob.Mana - manaDrain);
                    mob.SendAsciiMessage("You feel your magical energy being drained!");
                    mob.PlaySound(0x1F8);
                    mob.FixedParticles(0x374A, 10, 15, 5013, EffectLayer.CenterFeet);
                }

                Say("Your power is mine!");
                _NextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }

            // ——— Terror Aura ———
            if (DateTime.UtcNow >= _NextTerror)
            {
                var list = AcquireIndirectTargets(Location, 10).ToList();

                foreach (var mob in list)
                {
                    if (mob.CheckSkill(SkillName.MagicResist, 0, 100.0 - (Int / 20)))
                    {
                        mob.SendAsciiMessage("You resist the wave of terror!");
                    }
                    else
                    {
                        mob.SendAsciiMessage("You are struck with terror!");
                        mob.Combatant = null;
                        mob.Direction = Utility.GetDirection(mob, this);
                        mob.Freeze(TimeSpan.FromSeconds(3.0));
                    }
                    mob.PlaySound(0x217);
                }

                if (Utility.RandomDouble() < 0.25)
                    Say("Despair!");

                _NextTerror = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        private IEnumerable<Mobile> AcquireIndirectTargets(Point3D p, int range)
        {
            var list = new List<Mobile>();
            var eable = Map.GetMobilesInRange(p, range);

            foreach (Mobile m in eable)
            {
                if (m != this && m.AccessLevel == AccessLevel.Player && CanBeHarmful(m))
                    list.Add(m);
            }

            eable.Free();
            return list;
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems);

            if (Utility.RandomDouble() < 0.05)
                PackItem(new ChestOfThePaleLegion());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadEncodedInt();
        }
    }
}
