using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;

namespace Server.Mobiles
{
    [CorpseName("a veradrix corpse")]
    public class Veradrix : BaseCreature
    {
        private DateTime _NextCorruptedBlast;
        private DateTime _NextShadowStep;
        private DateTime _NextCorruptingField;

        // Cooldowns & parameters
        private static readonly TimeSpan CorruptedBlastWindup   = TimeSpan.FromSeconds(2.5);
        private static readonly TimeSpan CorruptedBlastCooldown = TimeSpan.FromSeconds(10.0);
        private const int CorruptedBlastRadius    = 5;
        private const int CorruptedBlastMinDamage = 40;
        private const int CorruptedBlastMaxDamage = 60;

        private static readonly TimeSpan ShadowStepCooldown       = TimeSpan.FromSeconds(20.0);
        private const int ShadowStepMinHealthPercent = 30;
        private const int ShadowStepRange            = 12;

        private static readonly TimeSpan CorruptingFieldCooldown = TimeSpan.FromSeconds(25.0);
        private const int CorruptingFieldRadius   = 6;
        private static readonly TimeSpan CorruptingFieldDuration = TimeSpan.FromSeconds(6.0);
        private const int CorruptingFieldStatDrain = 25;

        [Constructable]
        public Veradrix()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Veradrix";
            Body = 400;
            BaseSoundID = 0x474;
            Hue = 0x48F;

            SetStr(650, 750);
            SetDex(180, 200);
            SetInt(200, 230);

            SetHits(1500, 1800);
            SetDamage(18, 25);
            SetDamageType(ResistanceType.Physical, 100);

            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire,     60, 70);
            SetResistance(ResistanceType.Cold,     65, 75);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   70, 80);

            SetSkill(SkillName.MagicResist, 110.0, 130.0);
            SetSkill(SkillName.Tactics,     120.0, 140.0);
            SetSkill(SkillName.Wrestling,   110.0, 130.0);
            SetSkill(SkillName.Anatomy,     110.0, 130.0);
            SetSkill(SkillName.Parry,       110.0, 130.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 75;
        }

        public Veradrix(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer => true;
        public override bool ShowFameTitle => false;

        // ——— Corrupted Blast ———
        public void DoCorruptedBlast(Mobile target)
        {
            if (Deleted || target == null || target.Deleted || target.Map != Map || !InRange(target, CorruptedBlastRadius + 2) || !CanBeHarmful(target))
                return;

            Animate(20, 5, 1, true, false, 0);

            // windup on head
            FixedParticles(0x3709, 1, 10, Hue, 0, 9502, EffectLayer.Head);
            PlaySound(0x65A);

            Timer.DelayCall(CorruptedBlastWindup, () =>
            {
                if (Deleted || target == null || target.Deleted || target.Map != Map)
                    return;

                // impact on the target
                Effects.SendLocationEffect(target.Location, target.Map, 0x3709, 16, 10, Hue, 0);
                Effects.PlaySound(target.Location, target.Map, 0x20F);

                var affected = new List<Mobile>();
                foreach (var m in Map.GetMobilesInRange(target.Location, CorruptedBlastRadius))
                {
                    if (m == this || !CanBeHarmful(m))
                        continue;

                    affected.Add(m);
                }

                foreach (var m in affected)
                {
                    DoHarmful(m);
                    int raw = Utility.RandomMinMax(CorruptedBlastMinDamage, CorruptedBlastMaxDamage);
                    int res = m.GetResistance(ResistanceType.Energy);
                    int dmg = Math.Max(1, (int)(raw * (1.0 - res / 100.0)));
                    AOS.Damage(m, this, dmg, 0, 0, 0, 0, 100);
                }
            });
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target &&
                DateTime.UtcNow >= _NextCorruptedBlast &&
                target.Map == Map &&
                InRange(target, CorruptedBlastRadius + 2) &&
                CanBeHarmful(target) &&
                InLOS(target))
            {
                DoCorruptedBlast(target);
                _NextCorruptedBlast = DateTime.UtcNow + CorruptedBlastWindup + CorruptedBlastCooldown;
            }

            if (DateTime.UtcNow >= _NextCorruptingField)
            {
                var toDebuff = new List<Mobile>();
                foreach (var m in Map.GetMobilesInRange(Location, CorruptingFieldRadius))
                {
                    if (m == this || !CanBeHarmful(m))
                        continue;
                    if (m.Player || m.IsControlled())
                        toDebuff.Add(m);
                }

                if (toDebuff.Count > 0)
                {
                    DoCorruptingField(toDebuff);
                    _NextCorruptingField = DateTime.UtcNow + CorruptingFieldCooldown;
                }
            }
        }

        // ——— Shadow Step ———
        public override void OnDamage(int amount, Mobile from, bool willKill)
        {
            base.OnDamage(amount, from, willKill);

            // break out early if any of these
            if (willKill) return;
            if (Deleted)   return;
            if (from == null || from.Deleted) return;
            if (Combatant == null)           return;

            if (DateTime.UtcNow < _NextShadowStep)
                return;

            if ((double)Hits / HitsMax > ShadowStepMinHealthPercent / 100.0)
                return;

            // find a random valid point
            Point3D dest = Point3D.Zero;
            for (int i = 0; i < 10; i++)
            {
                int x = X + Utility.RandomMinMax(-ShadowStepRange, ShadowStepRange);
                int y = Y + Utility.RandomMinMax(-ShadowStepRange, ShadowStepRange);
                var p = new Point3D(x, y, Z);

                if (Map.CanSpawnMobile(p))
                {
                    dest = p;
                    break;
                }
            }

            if (dest == Point3D.Zero)
                return;

            // vanish
            FixedParticles(0x3728, 10, 10, Hue,  0, 9502, EffectLayer.CenterFeet);
            PlaySound(0x655);

            MoveToWorld(dest, Map);

            // reappear
            FixedParticles(0x3728, 10, 10, Hue,  0, 9502, EffectLayer.CenterFeet);
            PlaySound(0x655);

            if (from is Mobile mAtt)
                mAtt.SendMessage("Veradrix vanishes and reappears elsewhere!");

            _NextShadowStep = DateTime.UtcNow + ShadowStepCooldown;
        }

        // ——— Corrupting Field ———
        public void DoCorruptingField(List<Mobile> affected)
        {
            if (Deleted) return;

            FixedParticles(0x3709, 20, 10, Hue, 0, 9502, EffectLayer.CenterFeet);
            PlaySound(0x20F);

            foreach (var m in affected)
            {
                if (m.Deleted) continue;
                DoHarmful(m);

                m.Mana = (int)(m.Mana * (1.0 - CorruptingFieldStatDrain / 100.0));
                m.Stam = (int)(m.Stam * (1.0 - CorruptingFieldStatDrain / 100.0));
                m.SendMessage("You feel Veradrix's corrupting field sap your strength!");
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 3);
            AddLoot(LootPack.Gems, 5);
            AddLoot(LootPack.Potions, 6);
            AddLoot(LootPack.Rich);

            if (Utility.RandomDouble() < 0.02)
            {
                // Example: PackItem(new Artifact_VeradrixCore());
            }
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write(1); // version

            writer.Write(_NextCorruptedBlast);
            writer.Write(_NextShadowStep);
            writer.Write(_NextCorruptingField);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            if (version >= 1)
            {
                _NextCorruptedBlast  = reader.ReadDateTime();
                _NextShadowStep      = reader.ReadDateTime();
                _NextCorruptingField = reader.ReadDateTime();
            }
            else
            {
                _NextCorruptedBlast  =
                _NextShadowStep      =
                _NextCorruptingField = DateTime.UtcNow;
            }
        }
    }
}
