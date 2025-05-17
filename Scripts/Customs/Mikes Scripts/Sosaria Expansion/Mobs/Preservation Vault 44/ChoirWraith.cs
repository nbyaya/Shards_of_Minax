using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("a choir wraith corpse")]
    public class ChoirWraith : BaseCreature
    {
        // Timers for special abilities
        private DateTime m_NextChorusTime;
        private DateTime m_NextWailTime;
        private DateTime m_NextEchoTime;
        private Point3D m_LastLocation;

        // Unique ghostly hue
        private const int UniqueHue = 1360; // pale lavender-blue

        [Constructable]
        public ChoirWraith() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name = "a Choir Wraith";
            Body = 30;                 // Harpy body
            BaseSoundID = 402;         // Harpy sounds
            Hue = UniqueHue;

            // —— Stats —— 
            SetStr(150, 200);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(1000, 1200);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(20, 30);

            // Primarily sonic/energy damage
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Energy, 90);

            // Resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 50, 60);
            SetResistance(ResistanceType.Poison, 25, 35);
            SetResistance(ResistanceType.Energy, 75, 85);

            // Skills
            SetSkill(SkillName.EvalInt, 110.0, 125.0);
            SetSkill(SkillName.Magery, 110.0, 125.0);
            SetSkill(SkillName.MagicResist, 115.0, 130.0);
            SetSkill(SkillName.Meditation, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 80;
            ControlSlots = 5;

            // Initialize ability cooldowns
            var now = DateTime.UtcNow;
            m_NextChorusTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextWailTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextEchoTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_LastLocation   = this.Location;

            // Loot
            PackItem(new Nightshade(Utility.RandomMinMax(10, 15)));
            PackItem(new BlackPearl(Utility.RandomMinMax(10, 15)));
            if (Utility.RandomDouble() < 0.02)
                PackItem(new VoiceguardOfValor()); // unique artifact
        }

        // —— Aura: Spectral Echoes —— 
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);

                    // Drain a bit of stamina and stun briefly
                    int stamDrain = Utility.RandomMinMax(5, 10);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage(0x22, "You reel from ghostly echoes!");
                        target.FixedParticles(0x376A, 8, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        target.Paralyze(TimeSpan.FromSeconds(1.0)); 
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();
            if (Combatant == null || Map == null || !Alive) return;

            var now = DateTime.UtcNow;

            // Priority: Chorus → Wail → Echo Burst
            if (now >= m_NextChorusTime && InRange(Combatant.Location, 8))
            {
                ChorusOfDissonance();
                m_NextChorusTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (now >= m_NextWailTime && InRange(Combatant.Location, 12))
            {
                HarmoniousWail();
                m_NextWailTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
            else if (now >= m_NextEchoTime)
            {
                EchoBurst();
                m_NextEchoTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
        }

        // —— AoE Sonic Blast + stun chance —— 
        private void ChorusOfDissonance()
        {
            this.Say("*A haunting chorus rises!*");
            PlaySound(916);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3779, 12, 25, UniqueHue);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(this.Location, 5))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var t in targets)
            {
                DoHarmful(t);
                int dmg = Utility.RandomMinMax(40, 60);
                AOS.Damage(t, this, dmg, 0, 0, 0, 0, 100);

                // 30% chance to stun
                if (Utility.RandomDouble() < 0.30)
                    t.Paralyze(TimeSpan.FromSeconds(1.5));
            }
        }

        // —— Single‑target Dark Hymn: heavy damage + mana drain —— 
        private void HarmoniousWail()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            this.Say("*Feel the final verse!*");
            PlaySound(917);
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36D4, 7, 0, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);

            int dmg = Utility.RandomMinMax(70, 90);
            DoHarmful(target);
            AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);

            // Drain up to 50 mana
            if (target.Mana > 0)
            {
                int drain = Utility.RandomMinMax(20, 50);
                target.Mana = Math.Max(0, target.Mana - drain);
                target.SendMessage(0x22, "Your mind is shattered by the wail!");
                target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // —— Narrow sonic spike on-ground hazard —— 
        private void EchoBurst()
        {
            if (!(Combatant is Mobile target) || !CanBeHarmful(target, false)) return;

            // Send a small echo at the target location
            PlaySound(918);
            Effects.SendLocationParticles(
                EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration),
                0x3728, 8, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (this.Map == null) return;

                var spawn = target.Location;
                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                // Use a VortexTile as a swirling echo hazard
                var tile = new VortexTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(spawn, this.Map);

            });
        }

        // —— Death: spawn multiple echo vortices —— 
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            this.Say("*The chorus... fades...*");
            PlaySound(917);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int xOff = Utility.RandomMinMax(-3, 3);
                int yOff = Utility.RandomMinMax(-3, 3);
                var loc = new Point3D(X + xOff, Y + yOff, Z);

                if (!Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                    loc.Z = Map.GetAverageZ(loc.X, loc.Y);

                var tile = new VortexTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(loc, this.Map);
            }
        }

        // —— Standard overrides & loot level —— 
        public override bool BleedImmune    => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 140.0;
        public override double DispelFocus      => 70.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
        }

        public override int GetAttackSound() => 916;
        public override int GetAngerSound()  => 916;
        public override int GetDeathSound()  => 917;
        public override int GetHurtSound()   => 919;
        public override int GetIdleSound()   => 918;

        // —— Serialization —— 
        public ChoirWraith(Serial serial) : base(serial) { }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers after load
            var now = DateTime.UtcNow;
            m_NextChorusTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 14));
            m_NextWailTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextEchoTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_LastLocation   = this.Location;
        }
    }
}
