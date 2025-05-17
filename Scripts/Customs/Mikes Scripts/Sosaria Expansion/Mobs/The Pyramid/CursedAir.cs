using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells.Seventh; // for Chain Lightning effect
using Server.Spells;        // for particle helpers

namespace Server.Mobiles
{
    [CorpseName("a cursed air corpse")]
    public class CursedAir : BaseCreature
    {
        private DateTime m_NextGaleTime;
        private DateTime m_NextMiasmaTime;
        private DateTime m_NextStaticTime;
        private Point3D m_LastLocation;
        private const int UniqueHue = 1175; // pale, eerie green

        [Constructable]
        public CursedAir() : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.3)
        {
            Name = "Cursed Air";
            Body = 13;               // same as basic Air Elemental
            BaseSoundID = 655;
            Hue = UniqueHue;

            // Stats
            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(400, 450);

            SetHits(1000, 1200);
            SetStam(200, 250);
            SetMana(500, 600);

            SetDamage(10, 15);
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold, 40);
            SetDamageType(ResistanceType.Energy, 50);

            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 80, 90);

            SetSkill(SkillName.EvalInt,       100.1, 110.0);
            SetSkill(SkillName.Magery,        100.1, 110.0);
            SetSkill(SkillName.MagicResist,   120.1, 130.0);
            SetSkill(SkillName.Meditation,     80.0,  90.0);
            SetSkill(SkillName.Tactics,        90.1, 100.0);
            SetSkill(SkillName.Wrestling,      80.1,  90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Initialize cooldowns
            m_NextGaleTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextStaticTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Starter loot
            PackItem(new BlackPearl(Utility.RandomMinMax(5, 10)));
            PackItem(new Nightshade(Utility.RandomMinMax(5, 10)));
        }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == null || m == this || !Alive || !m.Alive || !m.InRange(this.Location, 3) || !CanBeHarmful(m, false))
                return;

            // Aura: Stamina drain and chilling gust
            if (m is Mobile target)
            {
                DoHarmful(target);
                int stamDrain = Utility.RandomMinMax(10, 20);
                if (target.Stam >= stamDrain)
                {
                    target.Stam -= stamDrain;
                    target.SendMessage(0x3F, "A chilling gust saps your stamina!");
                    target.FixedParticles(0x37B9, 10, 15, 5032, EffectLayer.Head);
                    target.PlaySound(0x2F3);
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (!Alive || Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Leave cursed vortex behind as it moves
            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.20)
            {
                var old = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    var vortex = new VortexTile();
                    vortex.Hue = UniqueHue;
                    vortex.MoveToWorld(old, Map);
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }

            // Cooldown-based special attacks
            DateTime now = DateTime.UtcNow;

            if (now >= m_NextGaleTime && this.InRange(Combatant.Location, 8))
            {
                SpectralGaleAttack();
                m_NextGaleTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (now >= m_NextMiasmaTime)
            {
                CorruptingMiasma();
                m_NextMiasmaTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
            else if (now >= m_NextStaticTime && this.InRange(Combatant.Location, 12))
            {
                StaticDischarge();
                m_NextStaticTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        private void SpectralGaleAttack()
        {
            Say("*Feel the cursed winds!*");
            PlaySound(0x1F1);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 12, 20, UniqueHue, 0, 5030, 0);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false))
                    targets.Add(m);
            }

            foreach (Mobile t in targets)
            {
                if (t is Mobile target)
                {
                    DoHarmful(target);
                    int dmg = Utility.RandomMinMax(30, 50);
                    AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);

                    // Knockback
                    int dx = target.X - X;
                    int dy = target.Y - Y;
                    dx = dx == 0 ? Utility.RandomMinMax(-1, 1) : dx / Math.Abs(dx);
                    dy = dy == 0 ? Utility.RandomMinMax(-1, 1) : dy / Math.Abs(dy);
                    var dest = new Point3D(target.X + dx * 2, target.Y + dy * 2, target.Z);
                    if (Map.CanFit(dest.X, dest.Y, dest.Z, target.Body, false, false))
                        target.Location = dest;
                }
            }
        }

        private void CorruptingMiasma()
        {
            Say("*Miasma rises!*");
            PlaySound(0x228);
            for (int i = 0; i < 6; i++)
            {
                int xOff = Utility.RandomMinMax(-4, 4);
                int yOff = Utility.RandomMinMax(-4, 4);
                var loc = new Point3D(X + xOff, Y + yOff, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var gas = new ToxicGasTile();
                gas.Hue = UniqueHue;
                gas.MoveToWorld(loc, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                    0x3709, 8, 15, UniqueHue, 0, 5031, 0);
            }
        }

        private void StaticDischarge()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false))
                return;

            Say("*Static surge!*");
            PlaySound(0x1F7);

            // Chain to up to 4 additional nearby targets
            List<Mobile> chain = new List<Mobile> { target };
            int max = 4, range = 6;
            for (int i = 0; i < max; i++)
            {
                var last = chain[chain.Count - 1];
                Mobile next = null;
                double best = double.MaxValue;

                foreach (Mobile m in Map.GetMobilesInRange(last.Location, range))
                {
                    if (m != this && !chain.Contains(m) && m.Alive && CanBeHarmful(m, false) && last.InLOS(m))
                    {
                        double d = last.GetDistanceToSqrt(m);
                        if (d < best)
                        {
                            best = d;
                            next = m;
                        }
                    }
                }

                if (next == null) break;
                chain.Add(next);
            }

            // Visual & damage
            for (int i = 0; i < chain.Count; i++)
            {
                var src = (i == 0 ? this : chain[i - 1]);
                var dst = chain[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, src.Location, src.Map),
                    new Entity(Serial.Zero, dst.Location, dst.Map),
                    0x3F61, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.1), () =>
                {
                    if (dst.Alive && CanBeHarmful(dst, false))
                    {
                        DoHarmful(dst);
                        int dmg = Utility.RandomMinMax(20, 30);
                        AOS.Damage(dst, this, dmg, 0, 0, 0, 0, 100);
                        dst.FixedParticles(0x374A, 5, 15, 9502, EffectLayer.Waist);
                    }
                });
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

			if (Map == null)
				return;

            Say("*The cursed winds... subside...*");
            PlaySound(0x214);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 60, UniqueHue, 0, 5052, 0);

            // Drop lingering hazards
            int count = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < count; i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);
                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var storm = new LightningStormTile();
                storm.Hue = UniqueHue;
                storm.MoveToWorld(loc, Map);
            }
        }

        // Properties & loot
        public override bool BleedImmune            => true;
        public override int TreasureMapLevel       => 5;
        public override double DispelDifficulty    => 130.0;
        public override double DispelFocus         => 65.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
            AddLoot(LootPack.MedScrolls);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 8));

            if (Utility.RandomDouble() < 0.01) // 1% cloak drop
                PackItem(new CloakOfWinds());
        }

        public CursedAir(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers on reload
            m_NextGaleTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextMiasmaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextStaticTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
        }
    }
}
