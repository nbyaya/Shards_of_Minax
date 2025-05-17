using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;       // for spell effects

namespace Server.Mobiles
{
    [CorpseName("an ethereal citizen corpse")]
    public class EtheralCitizen : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextPhaseTime;
        private DateTime m_NextChainsTime;
        private DateTime m_NextWailTime;
        private DateTime m_NextMirrorTime;

        // Last position for leaving phantom tiles
        private Point3D m_LastLocation;

        // A ghostly cyan hue
        private const int UniqueHue = 1166;

        [Constructable]
        public EtheralCitizen()
            : base(AIType.AI_NecroMage, FightMode.Closest, 12, 1, 0.2, 0.4)
        {
            Name           = "an Etheral Citizen";
            Body           = 252;       // same body as the Wight
            BaseSoundID    = 0x482;     // same base sound
            Hue            = UniqueHue;

            // Vastly increased stats
            SetStr(350, 400);
            SetDex(120, 140);
            SetInt(450, 500);

            SetHits(1200, 1400);
            SetStam(200, 250);
            SetMana(800, 900);

            SetDamage(18, 26);

            // Damage profile: mostly cold & energy
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Cold,     50);
            SetDamageType(ResistanceType.Energy,   40);

            // Resistances
            SetResistance(ResistanceType.Physical, 45, 55);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     70, 80);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   75, 85);

            // Skills
            SetSkill(SkillName.Necromancy,   120.0, 130.0);
            SetSkill(SkillName.Magery,       110.0, 120.0);
            SetSkill(SkillName.EvalInt,      115.0, 125.0);
            SetSkill(SkillName.MagicResist,  120.0, 130.0);
            SetSkill(SkillName.Meditation,   100.0, 110.0);
            SetSkill(SkillName.Tactics,      100.0, 110.0);
            SetSkill(SkillName.Wrestling,     90.0, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 70;
            ControlSlots = 4;

            // Schedule first uses of abilities
            m_NextPhaseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextWailTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextMirrorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Base loot
            PackItem(new Bone(Utility.RandomMinMax(20, 30)));
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 25)));
            PackItem(new Bloodmoss(Utility.RandomMinMax(15, 25)));
        }

        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Lethal;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 80.0;

        // Leave a phantom echo tile when moving
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            if (this.Location != m_LastLocation && Utility.RandomDouble() < 0.2)
            {
                Point3D dropLoc = m_LastLocation;
                Map map = this.Map;

                if (map != null && map.CanFit(dropLoc.X, dropLoc.Y, dropLoc.Z, 16, false, false))
                {
                    VortexTile tile = new VortexTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(dropLoc, map);
                }
                m_LastLocation = this.Location;
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            // Spectral Phase: short invisibility and immaterial movement
            if (DateTime.UtcNow >= m_NextPhaseTime && InRange(Combatant.Location, 12))
            {
                SpectralPhase();
                m_NextPhaseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
                return;
            }

            // Soul Chains: root and periodic damage
            if (DateTime.UtcNow >= m_NextChainsTime && InRange(Combatant.Location, 8))
            {
                SoulChains();
                m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
                return;
            }

            // Wailing Dirge: AoE stun and cold damage
            if (DateTime.UtcNow >= m_NextWailTime)
            {
                WailingDirge();
                m_NextWailTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
                return;
            }

            // Mirror Veil: chance to reflect incoming spells (stubbed)
            if (DateTime.UtcNow >= m_NextMirrorTime)
            {
                MirrorVeil();
                m_NextMirrorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
        }

        private void SpectralPhase()
        {
            this.Say("*Shadows coalesce...*");
            this.Hidden = true;    // go invisible
            // after 6 seconds, reveal again
            Timer.DelayCall(TimeSpan.FromSeconds(6), () =>
            {
                if (Alive)
                {
                    this.Hidden = false;
                    this.RevealingAction();  // optional, plays the reveal effect
                }
            });
        }

        private void SoulChains()
        {
            if (!(Combatant is Mobile target)) return;

            this.Say("*Bound by anguish!*");
            DoHarmful(target);

            target.PlaySound(0x3EC);
            target.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);

            // Paralyze to root in place
            target.Paralyze(TimeSpan.FromSeconds(4));

            // Ongoing damage
            Timer.DelayCall(TimeSpan.FromSeconds(1), () =>
            {
                if (target.Alive && this.Alive)
                    AOS.Damage(target, this, Utility.RandomMinMax(25, 35), 0, 30, 0, 0, 70);
            });
        }

        private void WailingDirge()
        {
            this.Say("*Hear my lament!*");
            PlaySound(0x482);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x376A, 12, 60, UniqueHue, 0, 5039, 0
            );

            var list = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false))
                    list.Add(m);
            }

            foreach (var m in list)
            {
                DoHarmful(m);
                m.Freeze(TimeSpan.FromSeconds(2));
                AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 50, 0, 0, 50);
            }
        }

        private void MirrorVeil()
        {
            this.Say("*Reflect my will...*");

            // ==== STUB: your spell-reflection logic goes here ====
            // e.g. set a flag for a few seconds and then override OnIncomingSpell or similar.
            // For now, this just plays a bubble effect:
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x375A, 10, 15, UniqueHue, 0, 5037, 0
            );
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            this.Say("*Your world dims...*");
            Effects.PlaySound(Location, Map, 0x211);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 12, 80, UniqueHue, 0, 5052, 0
            );

            // Scatter spectral shards
            for (int i = 0; i < 5; i++)
            {
                var offset = new Point3D(
                    X + Utility.RandomMinMax(-3, 3),
                    Y + Utility.RandomMinMax(-3, 3),
                    Z
                );

                if (Map.CanFit(offset.X, offset.Y, offset.Z, 16, false, false))
                {
                    ManaDrainTile shard = new ManaDrainTile();
                    shard.Hue = UniqueHue;
                    shard.MoveToWorld(offset, Map);
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems,      Utility.RandomMinMax(8, 12));
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));

            // 3% chance for a unique artifact
            if (Utility.RandomDouble() < 0.03)
                PackItem(new MaxxiaScroll()); // placeholder for real artifact
        }

        public EtheralCitizen(Serial serial) : base(serial) { }

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
            m_NextPhaseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextWailTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextMirrorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
