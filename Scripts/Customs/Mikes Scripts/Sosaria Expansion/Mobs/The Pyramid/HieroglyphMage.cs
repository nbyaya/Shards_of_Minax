using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Targeting;

namespace Server.Mobiles
{
    [CorpseName("the ashes of a hieroglyph mage")]
    public class HieroglyphMage : BaseCreature
    {
        // Cooldown timers
        private DateTime m_NextSandstorm;
        private DateTime m_NextGlyphBind;
        private DateTime m_NextSummon;
        private Point3D m_LastLocation;

        // A golden‐sandy glyph hue
        private const int UniqueHue = 2503;

        [Constructable]
        public HieroglyphMage() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a hieroglyph mage";
            Body = 85;
            BaseSoundID = 639;
            Hue = UniqueHue;

            // Stats
            SetStr(350, 400);
            SetDex(200, 240);
            SetInt(500, 550);

            SetHits(1200, 1400);
            SetMana(600, 700);
            SetStam(200, 250);

            SetDamage(10, 15);

            // Damage types
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 80, 90);

            // Skills
            SetSkill(SkillName.EvalInt, 115.0, 130.0);
            SetSkill(SkillName.Magery, 115.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 135.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 85.0, 95.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // Initial cooldowns
            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextGlyphBind  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextSummon    = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Pack reagents & rare pyramid loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));
            if (Utility.RandomDouble() < 0.05)
                PackItem(new ChesswardensResolve()); // a rare quest item
        }

        // --- Cursed Glyph Aura: saps  stamina when someone steps in range 2 ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m != null && m != this && m.Alive && m.Map == this.Map && m.InRange(this.Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int drain = Utility.RandomMinMax(5, 10);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x22, "The glyphs scorch your legs, sapping your strength!");
                        target.FixedParticles(0x3709, 10, 15, UniqueHue, EffectLayer.Waist);
                        target.PlaySound(0x20A);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            DateTime now = DateTime.UtcNow;
            int range = (int)GetDistanceToSqrt(Combatant.Location);

            if (now >= m_NextSandstorm && range <= 12)
            {
                SandstormBarrage();
                m_NextSandstorm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextGlyphBind && range <= 10)
            {
                GlyphOfBinding();
                m_NextGlyphBind = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (now >= m_NextSummon && range <= 15)
            {
                SummonMummies();
                m_NextSummon = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 50));
            }
        }

        // --- Ability 1: Sandstorm Barrage (AoE) ---
        private void SandstormBarrage()
        {
            this.Say("*Beneath the sands, strike!*");
            PlaySound(0x225);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3728, 12, 20, UniqueHue, 0, 5027, 0);

            List<Mobile> victims = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && m is Mobile target)
                    victims.Add(target);
            }
            eable.Free();

            foreach (Mobile tgt in victims)
            {
                DoHarmful(tgt);
                AOS.Damage(tgt, this, Utility.RandomMinMax(30, 50), 0, 0, 0, 0, 100);
                tgt.FixedParticles(0x374A, 8, 15, UniqueHue, EffectLayer.Waist);
            }
        }

        // --- Ability 2: Glyph of Binding (single target root + mana drain) ---
        private void GlyphOfBinding()
        {
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*Bound by ancient glyphs!*");
                PlaySound(0x1F7);
                target.Animate(17, 5, 1, true, false, 0);
                
                // Root
                target.Freeze(TimeSpan.FromSeconds(4.0));
                // Mana drain
                int drained = Utility.RandomMinMax(30, 50);
                if (target.Mana >= drained)
                {
                    target.Mana -= drained;
                    target.SendMessage(0x22, "Arcane runes siphon your magic!");
                    target.FixedParticles(0x373A, 10, 15, UniqueHue, EffectLayer.Head);
                }
            }
        }

        // --- Ability 3: Summon Mummified Guardians ---
        private void SummonMummies()
        {
            this.Say("*Rise, my guardians!*");
            PlaySound(0x1FE);

            for (int i = 0; i < 3; i++)
            {
                Point3D spawn = new Point3D(
                    this.X + Utility.RandomMinMax(-2, 2),
                    this.Y + Utility.RandomMinMax(-2, 2),
                    this.Z);

                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                var mummy = new Mummy(); // assume this class exists
                mummy.MoveToWorld(spawn, this.Map);
                mummy.Combatant = this.Combatant; // attack same target
            }
        }

        // --- Death Effect: Glyphic Implosion + Necromantic Flames ---
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map == null) return;

            this.Say("*The seals... broken!*");
            PlaySound(0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 12, 40, UniqueHue, 0, 5052, 0);

            // Scatter necromantic fire hazards
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tile = new NecromanticFlamestrikeTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // Standard overrides
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public HieroglyphMage(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            // Reset cooldowns
            m_NextSandstorm = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextGlyphBind  = DateTime.UtcNow + TimeSpan.FromSeconds(6);
            m_NextSummon     = DateTime.UtcNow + TimeSpan.FromSeconds(20);
        }
    }
}
