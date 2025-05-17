using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // For spell effects
using Server.Network;         // For effects and sounds
using Server.Spells.Fourth;   // For fire field spells

namespace Server.Mobiles
{
    [CorpseName("a burned savage corpse")]
    public class InfernalSavage : BaseCreature
    {
        // Unique fire hue for this advanced savage
        private const int UniqueHue = 1161;

        // Timers for special abilities
        private DateTime m_NextAuraTime;
        private DateTime m_NextCleaveTime;
        // To help leave a scorching trail as it moves
        private Point3D m_LastLocation;

        [Constructable]
        public InfernalSavage() 
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "an Infernal Savage";
            Hue = UniqueHue;

            // Use the same body as the basic savage
            if (this.Female = Utility.RandomBool())
                this.Body = 184;
            else
                this.Body = 183;

            // --- Boosted Stats ---
            // Advanced savage stats can be scaled higher than the basic savage:
            SetStr(250, 300);
            SetDex(150, 180);
            SetInt(80, 100);

            // Increased health pool for a boss‐like creature
            SetHits(800, 900);

            // Slightly increased damage output; note that its damage now is split as fire‐themed
            SetDamage(30, 35);
            // Let a majority of its damage be fire, with a minor physical component
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 85, 95);    // Very strong fire defense
            SetResistance(ResistanceType.Cold, -5, 5);       // Somewhat vulnerable to cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Enhanced Skills (melee oriented) ---
            SetSkill(SkillName.Fencing, 80.0, 100.0);
            SetSkill(SkillName.Macing, 80.0, 100.0);
            SetSkill(SkillName.Swords, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 80.0, 100.0);

            Fame = 10000;
            Karma = -10000;

            VirtualArmor = 40; 
            ControlSlots = 3;

            // Pack a few items for flavor and minor healing
            PackItem(new Bandage(Utility.RandomMinMax(1, 15)));

            // Optionally, pack a tribal or fire-inspired item (adjust item probabilities as needed)
            if (Utility.RandomDouble() < 0.1)
                PackItem(new TribalBerry());
            else if (Utility.RandomDouble() < 0.1)
                PackItem(new BolaBall());

            // Optional: Equip a melee weapon and rough bone armor for that savage look
            AddItem(new Spear());
            AddItem(new BoneArms());
            AddItem(new BoneLegs());

            // Chance for a distinctive mask upgrade
            if (Utility.RandomDouble() < 0.5)
                AddItem(new SavageMask());
            else if (Utility.RandomDouble() < 0.1)
                AddItem(new OrcishKinMask());

            // Initialize our ability timers
            m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(15 + Utility.RandomMinMax(5, 10));
            m_NextCleaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(10 + Utility.RandomMinMax(5, 10));
            m_LastLocation = this.Location;
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }
        public override int Meat { get { return 2; } }
        public override TribeType Tribe { get { return TribeType.Savage; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.SavagesAndOrcs; } }

        // --- Advanced Alteration for Melee Damage ---
        // (For example, extra damage against certain creature types)
        public override void AlterMeleeDamageTo(Mobile to, ref int damage)
        {
            if (to is Dragon || to is WhiteWyrm || to is SwampDragon || to is Drake || to is Nightmare || to is Hiryu || to is LesserHiryu || to is Daemon)
                damage *= 3;
        }

        // --- OnThink: Check for movement and special ability cooldowns ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave behind a scorching trail if the Infernal Savage has moved
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                // Create a fire field at the old location
                int itemID = 0x398C;  // Use an appropriate fire-field item graphic ID
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                int fieldDamage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, fieldDamage);
            }

            // Only check further if we have a combatant and are in a valid map
            if (Combatant == null || this.Map == null || this.Map == Map.Internal)
                return;

            // Always validate that Combatant is of type Mobile before accessing Mobile-specific members.
            Mobile target = null;
            if (Combatant is Mobile)
                target = Combatant as Mobile;

            if (target == null)
                return;

            double distance = GetDistanceToSqrt(target);

            // --- Burning Aura Attack ---
            if (DateTime.UtcNow >= m_NextAuraTime && distance <= 3)
            {
                BurningAuraAttack();
                m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(15 + Utility.RandomMinMax(5, 10));
            }
            // --- Flame Cleave Attack ---
            else if (DateTime.UtcNow >= m_NextCleaveTime && distance <= 2)
            {
                FlameCleaveAttack();
                m_NextCleaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(10 + Utility.RandomMinMax(5, 10));
            }
        }

        // --- Unique Ability: Burning Aura ---
        // A periodic attack that sears any foes within a 3-tile radius.
        public void BurningAuraAttack()
        {
            if (this.Map == null)
                return;

            // Play a fire-based sound effect
            this.PlaySound(0x208);
            // Emit a burst of fiery particles from the Infernal Savage
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 
                0x3709, 10, 20, UniqueHue, 0, 5029, 0);

            // Get every mobile within 3 tiles and apply fire damage
            IPooledEnumerable eable = this.Map.GetMobilesInRange(this.Location, 3);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(15, 25);
                    // Deal pure fire damage (physical = 0, fire = 100)
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                    // Additional visual effect on the affected target
                    m.FixedParticles(0x3779, 10, 20, 5032, UniqueHue, 0, 0);
                }
            }
            eable.Free();
        }

        // --- Unique Ability: Flame Cleave ---
        // When in very close range, the Infernal Savage unleashes a devastating AoE fire attack.
        public void FlameCleaveAttack()
        {
            if (this.Map == null)
                return;

            Mobile primaryTarget = Combatant as Mobile;
            if (primaryTarget == null)
                return;

            // Play a loud roar or explosion sound and show a burst of fiery particles on the primary target
            this.PlaySound(0x208);
            primaryTarget.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, 0);

            // Collect all valid targets within a 2-tile radius of the primary target
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = this.Map.GetMobilesInRange(primaryTarget.Location, 2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            // Deal a burst of fire damage to each affected target
            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(30, 50);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                m.FixedParticles(0x3779, 10, 20, 5032, UniqueHue, 0, 0);
            }
        }

        // --- OnDeath: Create a fiery explosion and spawn HotLavaTiles ---
        public override void OnDeath(Container c)
        {
            if (this.Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 8;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < lavaTilesToDrop - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D lavaLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!this.Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = this.Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!this.Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(lavaLocation);

                // Spawn a HotLavaTile (assumed to be defined elsewhere) at the location
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                // Create a brief flamestrike visual at the lava tile location
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Pick one of the locations for a big central explosion effect
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218); 
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 
                    0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Advanced Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Average, 2);

            // Chance for a unique drop, such as a fire-themed cloak or crest
            if (Utility.RandomDouble() < 0.02)
                PackItem(new MaxxiaScroll()); // Assume this item is defined elsewhere
        }

        // --- Serialization ---
        public InfernalSavage(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextAuraTime);
            writer.Write(m_NextCleaveTime);
            // Optionally, write m_LastLocation if needed
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextAuraTime = reader.ReadDateTime();
            m_NextCleaveTime = reader.ReadDateTime();
            m_LastLocation = this.Location;
        }
    }
}
