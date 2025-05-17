using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;

namespace Server.Mobiles
{
    [CorpseName("an ore‑scaled dragon corpse")]
    public class OreScaledDragon : BaseCreature
    {
        private DateTime m_NextBreathTime;
        private DateTime m_NextShardTime;
        private DateTime m_NextPulseTime;

        // Unique metallic bronze hue
        private const int UniqueHue = 2210;

        [Constructable]
        public OreScaledDragon()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an ore‑scaled dragon";
            Body = 0x31F;              // Swamp dragon body
            BaseSoundID = 362;         // Dragon roar
            Hue = UniqueHue;

            // Core stats
            SetStr(800, 900);
            SetDex(200, 250);
            SetInt(150, 200);

            SetHits(2000, 2400);
            SetStam(250, 300);
            SetMana(100, 150);

            SetDamage(25, 35);

            // Damage types
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 60, 70);

            // Skills
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Tactics,   100.1, 110.0);
            SetSkill(SkillName.MagicResist, 100.1, 110.0);

            Fame = 25000;
            Karma = -25000;
            VirtualArmor = 75;
            ControlSlots = 7;

            // Initialize ability cooldowns
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPulseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            // Mining-themed loot
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            PackItem(new DullCopperOre(Utility.RandomMinMax(20, 30)));
            PackGem();
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            var now = DateTime.UtcNow;

            // Lava‑like fire breath (AoE in 8‑tile radius)
            if (now >= m_NextBreathTime && InRange(Combatant.Location, 8))
            {
                BreathAttack();
                m_NextBreathTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // Shard Storm (drops landmine‐style ore shards)
            else if (now >= m_NextShardTime && InRange(Combatant.Location, 10))
            {
                ShardStorm();
                m_NextShardTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // Magnetic Pulse (drains stamina & slows)
            else if (now >= m_NextPulseTime && InRange(Combatant.Location, 6))
            {
                MagneticPulse();
                m_NextPulseTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            }
        }

        // --- Fire Breath: heavy fire damage in a radius ---
        private void BreathAttack()
        {
            this.Say("*ROOOAAAR!*");
            this.PlaySound(0x227);
            Effects.SendLocationParticles(
                EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 20, 30, UniqueHue, 0, 5028, 0);

            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 8))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(60, 90), 0, 0, 0, 0, 100);
            }
        }

        // --- Shard Storm: scatters ore shards as landmine tiles ---
        private void ShardStorm()
        {
            this.Say("*Metal rends the earth!*");
            this.PlaySound(0x2A3);

            int shards = Utility.RandomMinMax(6, 10);
            for (int i = 0; i < shards; i++)
            {
                int x = X + Utility.RandomMinMax(-6, 6);
                int y = Y + Utility.RandomMinMax(-6, 6);
                int z = Z;

                // Ensure valid placement
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var tile = new LandmineTile(); 
                tile.Hue = UniqueHue;
                tile.MoveToWorld(new Point3D(x, y, z), Map);

                Effects.SendLocationParticles(
                    EffectItem.Create(new Point3D(x, y, z), Map, EffectItem.DefaultDuration),
                    0x376A, 10, 15, UniqueHue, 0, 5039, 0);
            }
        }

        // --- Magnetic Pulse: drains stamina and applies slow ---
        private void MagneticPulse()
        {
            this.Say("*The earth itself drags you down!*");
            this.PlaySound(0x207);

            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);

                    if (m is Mobile target)
                    {
                        int drain = Utility.RandomMinMax(20, 35);
                        if (target.Stam >= drain)
                        {
                            target.Stam -= drain;
                            target.SendMessage(0x22, "You feel an overwhelming weight tether you!"); 
                            target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                        }

                        // Apply a brief slow
                        target.Combatant = this; // ensure hostility
                    }
                }
            }
        }

        public override void OnDeath(Container corpse)
        {
            base.OnDeath(corpse);

            // Cave‑in effect
            this.PlaySound(0x2A2);
            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3728, 20, 40, UniqueHue, 0, 5039, 0);

            // Spawn earth‑and‑lava hazards
            int count = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < count; i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = Z;

                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(new Point3D(x, y, z), Map);

                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Loot & Properties ---
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus    => 75.0;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // Chance for rare ore or artifact
            if (Utility.RandomDouble() < 0.05)
                PackItem(new IronOre(Utility.RandomMinMax(30, 50)));
        }

        public OreScaledDragon(Serial serial)
            : base(serial)
        {
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

            // Re‑initialize cooldowns
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextShardTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextPulseTime  = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
        }
    }
}
