using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("an ore bird corpse")]
    public class OreBird : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextMagnetTime;
        private DateTime m_NextShardTime;
        private DateTime m_NextDiveTime;
        private DateTime m_NextStompTime;

        // Unique metallic hue (coppery‑brown)
        private const int UniqueHue = 2053;

        [Constructable]
        public OreBird()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name           = "an ore bird";
            Body           = 0xD0;        // chicken body
            BaseSoundID    = 0x6E;        // chicken sounds
            Hue            = UniqueHue;

            // Stats
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(200, 250);

            SetHits(1500, 1800);
            SetStam(300, 350);
            SetMana(100, 200);

            SetDamage(20, 30);

            // Damage types: mostly physical with some cold (shards)
            SetDamageType(ResistanceType.Physical, 70);
            SetDamageType(ResistanceType.Cold, 30);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     50, 60);
            SetResistance(ResistanceType.Poison,   30, 40);
            SetResistance(ResistanceType.Energy,   40, 50);

            // Skills
            SetSkill(SkillName.Tactics,     100.1, 110.0);
            SetSkill(SkillName.Wrestling,   100.1, 110.0);
            SetSkill(SkillName.MagicResist,  80.1,  90.0);

            Fame       = 20000;
            Karma      = -15000;
            VirtualArmor = 70;

            ControlSlots = 5;  // boss-level creature

            // Initialize ability timers
            var now = DateTime.UtcNow;
            m_NextMagnetTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDiveTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextStompTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));

            // Loot: ore and metal
            PackItem(new IronOre(Utility.RandomMinMax(20, 30)));
            PackItem(new CopperOre(Utility.RandomMinMax(20, 30)));
            PackGold(300, 500);

            // Rare metal ingot drop
            if (Utility.RandomDouble() < 0.10) // 10% chance
                PackItem(new DullCopperIngot(Utility.RandomMinMax(1, 3)));
        }

        public OreBird(Serial serial)
            : base(serial)
        {
        }

        public override bool CanFly => true;

        public override int Meat     => 5;
        public override int Feathers => 0;
        public override MeatType MeatType => MeatType.Bird;
        public override FoodType FavoriteFood => FoodType.GrainsAndHay;

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            var now = DateTime.UtcNow;

            // Magnetic Pull: drags metal, drains stamina
            if (now >= m_NextMagnetTime && Combatant is Mobile magnetTarget && InRange(magnetTarget.Location, 8))
            {
                MagneticPull(magnetTarget);
                m_NextMagnetTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Shard Storm: AoE shard barrage + landmine traps
            else if (now >= m_NextShardTime && InRange(Combatant.Location, 12))
            {
                ShardStormAttack();
                m_NextShardTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 28));
            }
            // Drill Dive: leap onto a single target
            else if (now >= m_NextDiveTime && Combatant is Mobile diveTarget && InRange(diveTarget.Location, 6))
            {
                DrillDive(diveTarget);
                m_NextDiveTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
            // Earthquake Stomp: ground tremor around self
            else if (now >= m_NextStompTime)
            {
                EarthquakeStomp();
                m_NextStompTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Magnetic Pull: drains stamina and pulls target closer ---
        private void MagneticPull(Mobile target)
        {
            if (!CanBeHarmful(target, false)) return;
            DoHarmful(target);

            Say("*Kraaaaw!*");
            target.SendMessage(0x22, "You feel an irresistible metallic pull!");
            target.PlaySound(0x5A); // pull sound

            // Stamina drain
            int stamDrained = Utility.RandomMinMax(20, 40);
            if (target.Stam >= stamDrained)
                target.Stam -= stamDrained;

            // Particle from bird to target
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, this.Map),
                0x36D4, 5, 0, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0);
        }

        // --- Shard Storm: AoE damage + landmine traps ---
        private void ShardStormAttack()
        {
            Say("*Screee!*");
            PlaySound(0x2F3); // storm sound

            var targets = new List<Mobile>();
            foreach (Mobile m in Map.GetMobilesInRange(Location, 6))
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }

            foreach (var m in targets)
            {
                DoHarmful(m);
                int dmg = Utility.RandomMinMax(25, 45);
                AOS.Damage(m, this, dmg, 100, 0, 0, 0, 0);
                m.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }

            // Scatter landmine traps
            for (int i = 0; i < 5; i++)
            {
                int x = X + Utility.RandomMinMax(-4, 4);
                int y = Y + Utility.RandomMinMax(-4, 4);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var mine = new LandmineTile();
                mine.Hue = UniqueHue;
                mine.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Drill Dive: leap to target for heavy melee ---
        private void DrillDive(Mobile target)
        {
            if (!CanBeHarmful(target, false)) return;
            DoHarmful(target);

            Say("*Thunk!*");
            PlaySound(0x212); // heavy impact

            // Leap effect
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, target.Map),
                0x36F4, 7, 0, false, true, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

            // Teleport and damage
            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (!Alive || Map == null) return;
                MoveToWorld(target.Location, Map);
                target.Freeze(TimeSpan.FromSeconds(2.0));
                AOS.Damage(this, this, 20, 0, 0, 0, 0, 100);
            });
        }

        // --- Earthquake Stomp: spawns earthquake tiles around --- 
        private void EarthquakeStomp()
        {
            Say("*Kraaa!*");
            PlaySound(0x2A8); // rumble

            for (int i = 0; i < 6; i++)
            {
                int x = X + Utility.RandomMinMax(-3, 3);
                int y = Y + Utility.RandomMinMax(-3, 3);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var quake = new EarthquakeTile();
                quake.Hue = UniqueHue;
                quake.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Death effect: explode ore shards ---
        public override void OnDeath(Container corpse)
        {
            base.OnDeath(corpse);

            Say("*Krrraa!*");
            PlaySound(0x207); // explosion

            Effects.SendLocationParticles(
                EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                0x3709, 10, 30, UniqueHue, 0, 5032, 0);

            // Scatter hot lava hazards
            for (int i = 0; i < 4; i++)
            {
                int x = X + Utility.RandomMinMax(-2, 2);
                int y = Y + Utility.RandomMinMax(-2, 2);
                int z = Z;
                if (!Map.CanFit(x, y, z, 16, false, false))
                    z = Map.GetAverageZ(x, y);

                var lava = new HotLavaTile();
                lava.Hue = UniqueHue;
                lava.MoveToWorld(new Point3D(x, y, z), Map);
            }
        }

        // --- Loot & properties ---
        public override bool BleedImmune => true;
        public override int TreasureMapLevel => 6;
        public override double DispelDifficulty => 150.0;
        public override double DispelFocus     => 75.0;

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();

            // Re-init timers
            var now = DateTime.UtcNow;
            m_NextMagnetTime = now + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextShardTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextDiveTime   = now + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextStompTime  = now + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
        }
    }
}
