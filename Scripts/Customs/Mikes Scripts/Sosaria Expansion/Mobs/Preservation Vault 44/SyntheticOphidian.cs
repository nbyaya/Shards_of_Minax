using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Seventh; // for potential spell visuals
using Server.Targeting;     // for line attacks

namespace Server.Mobiles
{
    [CorpseName("a synthetic ophidian corpse")]
    public class SyntheticOphidian : BaseCreature
    {
        private static readonly string[] m_Names = new[]
        {
            "a synthetic ophidian",
            "an ophidian automaton",
            "a nanite ophidian"
        };

        private DateTime m_NextAcidSpit;
        private DateTime m_NextNeuralShock;
        private DateTime m_NextNaniteSwarm;
        private Point3D m_LastLocation;

        private const int UniqueHue = 1365; // Teal‐green metallic sheen

        [Constructable]
        public SyntheticOphidian()
            : base(AIType.AI_Melee, FightMode.Closest, 12, 1, 0.1, 0.2)
        {
            Name        = m_Names[Utility.Random(m_Names.Length)];
            Body        = 86;
            BaseSoundID = 634;
            Hue         = UniqueHue;

            SetStr(300, 350);
            SetDex(200, 250);
            SetInt(200, 250);

            SetHits(1200, 1400);
            SetStam(300, 350);
            SetMana(200, 250);

            SetDamage(20, 30);
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison,   50);

            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   70, 80);
            SetResistance(ResistanceType.Energy,   50, 60);

            SetSkill(SkillName.Swords,     100.1, 110.0);
            SetSkill(SkillName.Tactics,    100.1, 110.0);
            SetSkill(SkillName.MagicResist,100.1, 110.0);
            SetSkill(SkillName.Poisoning,  120.1, 130.0);
            SetSkill(SkillName.Anatomy,     90.1, 100.0);

            Fame            = 25000;
            Karma           = -25000;
            VirtualArmor   = 90;
            ControlSlots    = 6;

            // Schedule first use of unique abilities
            var now = DateTime.UtcNow;
            m_NextAcidSpit    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextNeuralShock = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextNaniteSwarm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));

            // Initial location marker
            m_LastLocation = Location;

            // Base loot
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new SpidersSilk(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));
        }

        public override bool BleedImmune  => true;
        public override Poison PoisonImmune => Poison.Deadly;
        public override int TreasureMapLevel => 7;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public SyntheticOphidian(Serial serial) : base(serial) { }

        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            base.OnMovement(m, oldLocation);

            // Neural Pulse: drains stamina from anyone who comes within 2 tiles
            if (m != this && m.Map == Map && m.InRange(Location, 2) && Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int drain = Utility.RandomMinMax(15, 25);
                    if (target.Stam >= drain)
                    {
                        target.Stam -= drain;
                        target.SendMessage(0x22, "You feel your legs falter as nanites disrupt your vitality!");
                        target.FixedParticles(0x374A, 8, 12, 1151, EffectLayer.Waist);
                        target.PlaySound(0x1F8);
                    }
                }
            }
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || !Alive || Map == null || Map == Map.Internal)
                return;

            var now = DateTime.UtcNow;

            // Acid Spit: line attack that leaves a corrosive trail
            if (now >= m_NextAcidSpit && InRange(Combatant.Location, 12))
            {
                AcidSpitLine();
                m_NextAcidSpit = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Neural Shock: AoE stamina drain + stun
            else if (now >= m_NextNeuralShock && InRange(Combatant.Location, 8))
            {
                NeuralShockAoE();
                m_NextNeuralShock = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }
            // Nanite Swarm: spawn hazardous nanite clouds around self
            else if (now >= m_NextNaniteSwarm)
            {
                NaniteSwarmBurst();
                m_NextNaniteSwarm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(40, 50));
            }
        }

        private void AcidSpitLine()
        {
            Mobile target = Combatant as Mobile;
			if (target == null || Map == null || Map == Map.Internal || !Alive)
				return;


            Say("*Hsss-splurt!*");
            PlaySound(0x228);

            // Send a moving particle from Self to target
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x36BD,  // acid bolt graphic
                9, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (!CanBeHarmful(target, false)) return;

                DoHarmful(target);
                int dmg = Utility.RandomMinMax(30, 45);
                AOS.Damage(target, this, dmg, 0, 0, 0, 0, 100);

                // Leave a corrosive PoisonTile at target location
                var loc = target.Location;
                if (Map.CanFit(loc.X, loc.Y, loc.Z, 16, false, false))
                {
                    var tile = new PoisonTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(loc, Map);
                }
            });
        }

        private void NeuralShockAoE()
        {
            Say("*>> STATIC DISRUPTION <<*");
            PlaySound(0x20B);

            var targets = new List<Mobile>();
            foreach (var m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && m.Alive && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                if (m is Mobile targ)
                {
                    DoHarmful(targ);

                    int stamDrain = Utility.RandomMinMax(20, 35);
                    if (targ.Stam >= stamDrain)
                    {
                        targ.Stam -= stamDrain;
                        targ.SendMessage(0x22, "Your muscles seize as synthetic coils disrupt your nerves!");
                        targ.FixedParticles(0x375A, 10, 15, 1151, EffectLayer.Waist);
                        targ.PlaySound(0x3C3);
                    }

                    // Small stun
                    targ.Freeze(TimeSpan.FromSeconds(1.5));

                    // Visual shock effect
                    targ.FixedParticles(0x3AAB, 5, 15, 0x482, EffectLayer.Head);
                }
            }
        }

        private void NaniteSwarmBurst()
        {
            Say("*Deploying swarm!*");
            PlaySound(0x22F);

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                var p = new Point3D(x, y, z);

                if (!Map.CanFit(x, y, z, 16, false, false))
                {
                    z = Map.GetAverageZ(x, y);
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        continue;
                    p = new Point3D(x, y, z);
                }

                var cloud = new ToxicGasTile(); // swirling nanite cloud
                cloud.Hue = UniqueHue;
                cloud.MoveToWorld(p, Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(p, Map, TimeSpan.FromSeconds(2)),
                    0x3728, 10, 20, UniqueHue, 0, 5044, 0);
            }
        }

        public override void OnDeath(Container c)
        {
            base.OnDeath(c);
			if (Map == null) return;

            Say("*Systems... failing...*");
            PlaySound(0x212);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, TimeSpan.FromSeconds(1)),
                0x36BD, 15, 30, UniqueHue, 0, 9513, 0);

            // Final nanite detonation: scatter PoisonTiles
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Z;
                var p = new Point3D(x, y, z);

                if (!Map.CanFit(x, y, z, 16, false, false))
                {
                    z = Map.GetAverageZ(x, y);
                    if (!Map.CanFit(x, y, z, 16, false, false))
                        continue;
                    p = new Point3D(x, y, z);
                }

                var tile = new PoisonTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(p, Map);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 14));
            AddLoot(LootPack.MedScrolls, Utility.RandomMinMax(2, 4));

            // Rare drop: Ophidian Nanite Core
            if (Utility.RandomDouble() < 0.03) // 3% chance
                PackItem(new ShadebindWrap());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Reset timers
            var now = DateTime.UtcNow;
            m_NextAcidSpit    = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextNeuralShock = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextNaniteSwarm = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
            m_LastLocation    = Location;
        }
    }
}
