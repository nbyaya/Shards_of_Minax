using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;      // for Particle effects, SpellHelper
using Server.Spells.Seventh; // if you want to base any barrage on Chain Lightning

namespace Server.Mobiles
{
    [CorpseName("an echo-wielder corpse")]
    public class EchoWielder : BaseCreature
    {
        // Unique violet hue for all effects/items
        private const int EchoHue = 1367;

        // Cooldowns for special abilities
        private DateTime m_NextReverberation;
        private DateTime m_NextPulse;
        private DateTime m_NextStorm;
        private Point3D m_LastLocation;

        [Constructable]
        public EchoWielder() 
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name           = "an Echo-Wielder";
            Body           = 0x190;        // same body as Zealot of Khaldun
            BaseSoundID    = 0x184;        // idle sound
            Hue            = EchoHue;

            // —— Stats —— (more powerful than a standard Summoner)
            SetStr(450, 550);
            SetDex(200, 250);
            SetInt(600, 700);

            SetHits(1800, 2100);
            SetStam(300, 350);
            SetMana(800, 900);

            SetDamage(10, 20);

            // Damage split: mostly sonic/energy
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Energy, 80);

            // Resistances
            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire,     50, 60);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   35, 45);
            SetResistance(ResistanceType.Energy,   85, 95);

            // Skills
            SetSkill(SkillName.EvalInt,   120.0, 135.0);
            SetSkill(SkillName.Magery,    120.0, 135.0);
            SetSkill(SkillName.MagicResist,125.0, 140.0);
            SetSkill(SkillName.Meditation,105.0, 115.0);
            SetSkill(SkillName.Tactics,    95.0, 105.0);
            SetSkill(SkillName.Wrestling,  95.0, 105.0);

            VirtualArmor = 90;
            Fame          = 25000;
            Karma         = -25000;
            ControlSlots  = 5;

            // Initialize cooldowns
            m_NextReverberation = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPulse         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextStorm         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;

            // Gear & loot hints (violet‑tinted)
            var gloves    = new LeatherGloves() { Hue = EchoHue }; AddItem(gloves);
            var boneHelm  = new BoneHelm()     { Hue = EchoHue }; AddItem(boneHelm);
            var necklace  = new Necklace()     { Hue = EchoHue }; AddItem(necklace);
            var cloak     = new Cloak()        { Hue = EchoHue }; AddItem(cloak);
            var kilt      = new Kilt()         { Hue = EchoHue }; AddItem(kilt);
            var sandals   = new Sandals()      { Hue = EchoHue }; AddItem(sandals);

            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
        }

        // Always aggressive, unprovokable, murder
        public override bool AlwaysMurderer => true;
        public override bool Unprovokable   => true;
        public override bool ClickTitle     => false;
        public override bool ShowFameTitle  => false;

        public override int GetIdleSound()  => 0x184;
        public override int GetAngerSound() => 0x286;
        public override int GetHurtSound()  => 0x19F;
        public override int GetDeathSound() => 0x288;

        // —— 1) Echo Aura —— (On movement, close targets get minor sonic damage + stun)
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (m == this || !Alive || !m.Alive || m.Map != this.Map || !m.InRange(Location, 2))
                return;

            if (m is Mobile target && CanBeHarmful(target, false))
            {
                DoHarmful(target);

                // Sonic jolt
                AOS.Damage(target, this, Utility.RandomMinMax(8, 15), 0, 0, 0, 0, 100);
                target.Freeze(TimeSpan.FromSeconds(0.8));
                Effects.PlaySound(target.Location, target.Map, 0x5C9);           // echo crackle
                target.FixedParticles(0x374A, 8, 12, EchoHue, EffectLayer.Head);
                target.SendMessage("You are knocked off balance by a reverberating pulse!");
            }
        }

        // —— Core AI loop handles three special attacks —— 
        public override void OnThink()
        {
            base.OnThink();

            // Leave behind an unstable echo‑vortex occasionally
            if (Map != null && Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                var old = m_LastLocation; m_LastLocation = Location;
                if (Map.CanFit(old.X, old.Y, old.Z, 16, false, false))
                {
                    var tile = new VortexTile { Hue = EchoHue };
                    tile.MoveToWorld(old, Map);
                }
            }
            else
            {
                m_LastLocation = Location;
            }

            if (Combatant == null || !Alive || Map == Map.Internal) return;

            // Sonic Reverberation (small AoE around self)
            if (DateTime.UtcNow >= m_NextReverberation && InRange(Combatant.Location, 8))
            {
                ReverberationShock();
                m_NextReverberation = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Focused Sonic Pulse (single‑target heavy hit + slow)
            else if (DateTime.UtcNow >= m_NextPulse && InRange(Combatant.Location, 12))
            {
                if (Combatant is Mobile target) PulseStrike(target);
                m_NextPulse = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Echo Storm (large delayed AoE at target’s feet)
            else if (DateTime.UtcNow >= m_NextStorm && Combatant is Mobile stormTarget && InRange(stormTarget.Location, 14))
            {
                EchoStorm(stormTarget.Location);
                m_NextStorm = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

        // —— Special #1: ReverberationShock — AoE energy burst around self
        private void ReverberationShock()
        {
            PlaySound(0x212); // deeper explosion
            FixedParticles(0x36BD, 12, 20, EchoHue, EffectLayer.CenterFeet);

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(30, 45), 0, 0, 0, 0, 100);
                m.Stam -= Utility.RandomMinMax(10, 20);
                m.FixedParticles(0x375A, 10, 15, EchoHue, EffectLayer.Waist);
                m.SendMessage("A wave of sonic energy rattles your mind!");
            }
        }

        // —— Special #2: PulseStrike — single‑target sonic hammer + slow
        private void PulseStrike(Mobile target)
        {
            PlaySound(0x213);
            Animate(12, 5, 1, true, false, 0); // staff‑slam animation
            DoHarmful(target);

            int damage = Utility.RandomMinMax(60, 80);
            AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
            target.Freeze(TimeSpan.FromSeconds(1.5));
            target.FixedParticles(0x3789, 8, 12, EchoHue, EffectLayer.Head);
            target.SendMessage("A concussive pulse slams into you, leaving you stunned!");
        }

        // —— Special #3: EchoStorm — delayed AoE hazard at X,Y
        private void EchoStorm(Point3D loc)
        {
            Say("*Resound!*");
            PlaySound(0x22F);

            // Warning effect
            Effects.SendLocationParticles(
                EffectItem.Create(loc, Map, EffectItem.DefaultDuration),
                0x3728, 8, 10, EchoHue, 0, 5039, 0);

            // After a short delay, drop multiple sonic hazard tiles
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                for (int i = 0; i < 5; i++)
                {
                    int dx = Utility.RandomMinMax(-2, 2);
                    int dy = Utility.RandomMinMax(-2, 2);
                    var spawn = new Point3D(loc.X + dx, loc.Y + dy, loc.Z);

                    if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                        spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                    var tile = new LightningStormTile { Hue = EchoHue };
                    tile.MoveToWorld(spawn, Map);

                }
            });
        }

        // —— Death: Shattering Crescendo — spawns ToxicGasTile & shards —— 
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            Say("*…echoes fade…*");
            Effects.PlaySound(Location, Map, 0x212);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 30, EchoHue, 0, 5052, 0);

            for (int i = 0; i < Utility.RandomMinMax(4, 7); i++)
            {
                int dx = Utility.RandomMinMax(-3, 3);
                int dy = Utility.RandomMinMax(-3, 3);
                var spawn = new Point3D(X + dx, Y + dy, Z);

                if (!Map.CanFit(spawn.X, spawn.Y, spawn.Z, 16, false, false))
                    spawn.Z = Map.GetAverageZ(spawn.X, spawn.Y);

                var gas = new ToxicGasTile { Hue = EchoHue };
                gas.MoveToWorld(spawn, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(spawn, Map, EffectItem.DefaultDuration),
                    0x3728, 8, 12, EchoHue, 0, 5039, 0);
            }
        }

        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 145.0;
        public override double DispelFocus     => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 14));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            
            if (Utility.RandomDouble() < 0.03)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(1, 2))); // rare drop
        }

        public EchoWielder(Serial serial) : base(serial) { }
        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }
        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset cooldowns on load
            m_NextReverberation = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPulse         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            m_NextStorm         = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            m_LastLocation      = this.Location;
        }
    }
}
