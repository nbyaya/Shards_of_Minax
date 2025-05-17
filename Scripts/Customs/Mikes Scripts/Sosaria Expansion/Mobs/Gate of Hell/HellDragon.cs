using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // For potential spell effects
using Server.Network;         // For effects and sounds
using Server.Spells.Fourth;   // For fire field effects

namespace Server.Mobiles
{
    [CorpseName("a hellish dragon corpse")]
    public class HellDragon : BaseCreature
    {
        // --- Cooldown timers for special abilities ---
        private DateTime m_NextBreathTime;
        private DateTime m_NextImpactTime;
        private DateTime m_NextRoarTime;
        private Point3D m_LastLocation;

        // --- Unique Hue for Hell Dragon (change as desired) ---
        private const int UniqueHue = 0x490;

        [Constructable]
        public HellDragon() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Hell Dragon";
            Body = 0x31A;            // Using the base body of the SwampDragon
            BaseSoundID = 0x16A;      // Using the base sound from SwampDragon
            Hue = UniqueHue;

            // --- Increased Attributes for a boss-level creature ---
            SetStr(600, 700);
            SetDex(150, 200);
            SetInt(400, 500);

            SetHits(2000, 2500);
            SetStam(250, 300);
            SetMana(400, 500);

            SetDamage(30, 40);

            // --- Damage Types: mostly fire with a dash of physical ---
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 0, 10);     // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Skills (high for a boss encounter) ---
            SetSkill(SkillName.EvalInt, 110.1, 130.0);
            SetSkill(SkillName.Magery, 110.1, 130.0);
            SetSkill(SkillName.MagicResist, 120.2, 140.0);
            SetSkill(SkillName.Tactics, 110.1, 130.0);
            SetSkill(SkillName.Wrestling, 110.1, 130.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 85;
            ControlSlots = 3;    // Adjust control slots since this is a tough, boss monster

            // --- Initialize ability cooldowns (staggered initial delays) ---
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextImpactTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRoarTime   = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_LastLocation = this.Location;

            // --- Standard loot items (plus potential extra drops) ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 8)));
        }

        // --- Unique Ability: Hellfire Breath ---
        // The Hell Dragon exhales a searing cone of flame to burn nearby foes.
        public void HellfireBreathAttack()
        {
            if (Map == null)
                return;

            // Ensure Combatant is a Mobile before proceeding
            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target))
                return;

            // Visual and audio effects for the breath
            PlaySound(0x160); // Fire-related sound
			FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist, 0);

            // Find targets in a roughly cone-shaped area (here simplified as an AoE radius)
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 7);
            foreach (Mobile m in eable)
            {
                if (m == this)
                    continue;

                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(35, 50); // High fire damage
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // 100% fire damage
                }
            }
        }

        // --- Unique Ability: Molten Impact ---
        // The Hell Dragon slams the ground to send a ring of blazing fire outward.
        public void MoltenImpactAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x2C7); // Ground impact sound
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 40, UniqueHue, 0, 5029, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 5);
            foreach (Mobile m in eable)
            {
                if (m == this)
                    continue;

                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(30, 45);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }
        }

        // --- Unique Ability: Scorching Roar ---
        // The Hell Dragon emits a fearsome roar that rattles and injures nearby foes.
        public void ScorchingRoarAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x218); // Roaring sound
            this.Say("*The Hell Dragon unleashes a scorching roar!*");

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m == this)
                    continue;

                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    
                    // If the target is a Mobile, hint at a dazed effect.
                    if (m is Mobile mobTarget)
                        mobTarget.SendMessage("You are dazed by the infernal roar!");
                }
            }
        }

        // --- Override OnMovement to create a burning trail ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                // Create a burning fire field at the mobile's previous location
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(3));
                int damage = 3;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);


		
                m.PlaySound(0x208);

                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Override OnThink to process special attack cooldowns ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Ensure Combatant is a Mobile before using its location
            Mobile target = Combatant as Mobile;
            if (target == null)
                return;

            // Hellfire Breath Attack if within 8 tiles and cooldown elapsed
            if (DateTime.UtcNow >= m_NextBreathTime && this.InRange(target.Location, 8))
            {
                HellfireBreathAttack();
                m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Molten Impact Attack if within 5 tiles and cooldown elapsed
            else if (DateTime.UtcNow >= m_NextImpactTime && this.InRange(target.Location, 5))
            {
                MoltenImpactAttack();
                m_NextImpactTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            // Scorching Roar Attack if within 6 tiles and cooldown elapsed
            else if (DateTime.UtcNow >= m_NextRoarTime && this.InRange(target.Location, 6))
            {
                ScorchingRoarAttack();
                m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Override OnDeath to spawn a death explosion with lava tiles ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 12;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(lavaLocation);

                // Spawn a HotLavaTile at the location (assuming HotLavaTile is defined elsewhere)
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218); // Big explosion sound
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        // Increased dispel difficulty for extra challenge
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 65.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

        }

        // --- Serialization ---
        public HellDragon(Serial serial) : base(serial)
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

            // Reinitialize ability cooldowns on load/restart
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextImpactTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextRoarTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
