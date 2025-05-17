using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Misc;

namespace Server.Mobiles
{
    [CorpseName("a skarnvex corpse")]
    public class Skarnvex : BaseCreature
    {
        private const int SkarnvexHue = 0x8A0;

        private DateTime m_NextWildernessAoE;
        private DateTime m_NextGraspingRoots;
        private DateTime m_NextCorrosiveSpit;

        [Constructable]
        public Skarnvex()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Body = 182;
            BaseSoundID = 0x45A;

            Name = "a Skarnvex";
            Hue = SkarnvexHue;

            SetStr(650, 750);
            SetDex(120, 150);
            SetInt(150, 200);

            SetHits(1000, 1200);
            SetDamage(15, 25);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 90.1, 105.0);
            SetSkill(SkillName.Tactics,    90.1, 105.0);
            SetSkill(SkillName.Wrestling,  95.1, 110.0);
            SetSkill(SkillName.Magery,     80.0,  95.0);

            Fame = 8000;
            Karma = -8000;

            VirtualArmor = 40;

            // Basic gold loot only; detailed loot is in GenerateLoot()
            PackGold(300, 500);

            // Chance for a crafting tool
            if (Utility.RandomDouble() < 0.15)
                PackItem(new Hatchet());   // concrete tool instead of abstract BaseTool

            // Chance for a power scroll
            if (Utility.RandomDouble() < 0.05)
                PackItem(new PowerScroll(SkillName.Wrestling, 110));

            m_NextWildernessAoE  = DateTime.UtcNow;
            m_NextGraspingRoots  = DateTime.UtcNow;
            m_NextCorrosiveSpit  = DateTime.UtcNow;
        }

        public Skarnvex(Serial serial) : base(serial) { }

        public override InhumanSpeech SpeechType => InhumanSpeech.Orc;
        public override bool CanRummageCorpses => true;

        public override int TreasureMapLevel => 4;
        public override int Meat            => 3;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich);
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.Gems);
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            var combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map ||
                !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
                return;

            if (DateTime.UtcNow >= m_NextWildernessAoE && Utility.RandomDouble() < 0.2)
            {
                WildernessShockwave(combatant);
                m_NextWildernessAoE = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }

            if (DateTime.UtcNow >= m_NextGraspingRoots && Utility.RandomDouble() < 0.15)
            {
                GraspingRoots(combatant);
                m_NextGraspingRoots = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 20));
            }

            if (DateTime.UtcNow >= m_NextCorrosiveSpit && Utility.RandomDouble() < 0.1)
            {
                CorrosiveSpit(combatant);
                m_NextCorrosiveSpit = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 15));
            }
        }

        public void WildernessShockwave(Mobile center)
        {
            if (center == null || center.Deleted || center.Map != Map || !InRange(center, 12))
                return;

            SendAsciiMessage("Skarnvex prepares a devastating shockwave!");
            PlaySound(BaseSoundID + 5);
            Animate(20, 5, 1, true, false, 0);

            DoHarmful(center);

            Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
            {
                if (center == null || center.Deleted || center.Map != Map || !InRange(center, 12))
                    return;

                PlaySound(0x21D);
                center.FixedParticles(0x376A, 9, 32, 5005, EffectLayer.Waist);

                const int aoeRange = 4;
                var targets = new List<Mobile>();
                foreach (Mobile m in center.GetMobilesInRange(aoeRange))
                    if (m != this && CanBeHarmful(m))
                        targets.Add(m);

                foreach (Mobile t in targets)
                {
                    DoHarmful(t);

                    int dmg = (t == center)
                        ? Utility.RandomMinMax(30, 45)
                        : Utility.RandomMinMax(25, 40);

                    int physRes = t.PhysicalResistance;
                    int reduced = Math.Max(1, (int)(dmg * (1.0 - physRes / 100.0)));

                    AOS.Damage(t, this, reduced, 100, 0, 0, 0, 0);

                    // Slow debuff via freeze
                    t.Freeze(TimeSpan.FromSeconds(5.0));
                }
            });
        }

        public void GraspingRoots(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 10) || !CanBeHarmful(target))
                return;

            SendAsciiMessage("Roots erupt from the ground!");
            PlaySound(0x2E3);
            DoHarmful(target);

            target.CantWalk = true;
            target.Freeze(TimeSpan.FromSeconds(3.0));
            target.ApplyPoison(this, Poison.Deadly);
            target.FixedParticles(0x373A, 10, 15, 5018, EffectLayer.Waist);
        }

        public void CorrosiveSpit(Mobile target)
        {
            if (target == null || target.Deleted || target.Map != Map || !InRange(target, 15) || !CanBeHarmful(target))
                return;

            SendAsciiMessage("Skarnvex spits corrosive goo!");
            PlaySound(0x162);
            DoHarmful(target);

            MovingParticles(target, 0x36D4, 7, 0, false, true, 0x3F, 0, 9502, 6014, 0x11D, EffectLayer.Waist, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.8), () =>
            {
                if (target == null || target.Deleted || target.Map != Map)
                    return;

                target.PlaySound(0x188);
                int dmg = Utility.RandomMinMax(10, 20);
                AOS.Damage(target, this, dmg, 0, 0, 0, 50, 50);

                var mods = new ResistanceMod[]
                {
                    new ResistanceMod(ResistanceType.Physical, -10),
                    new ResistanceMod(ResistanceType.Poison,   -15)
                };

                foreach (var mod in mods)
                    target.AddResistanceMod(mod);

                Timer.DelayCall(TimeSpan.FromSeconds(8.0), () =>
                {
                    if (target == null || target.Deleted || target.Map != Map)
                        return;

                    foreach (var mod in mods)
                        target.RemoveResistanceMod(mod);

                    target.SendAsciiMessage("The corrosive effect wears off.");
                });
            });
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            reader.ReadInt();
        }
    }
}
