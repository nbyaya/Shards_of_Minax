using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For spell effects
using Server.Network;        // For sending effects/sounds
using System.Collections.Generic; // For lists in AoE targeting
using Server.Spells.Fourth;  // For firefield effects

namespace Server.Mobiles
{
    [CorpseName("a smoldering charred corpse")]
    public class BurningGargoyle : BaseCreature
    {
        // Cooldown timers for special abilities to prevent spamming
        private DateTime m_NextFireScreamTime;
        private DateTime m_NextEmberStormTime;
        private DateTime m_NextWingFlapTime;

        // Used to leave a trail of fire when moving
        private Point3D m_LastLocation;

        // Unique hue for the Burning Gargoyle (choose as desired)
        private const int UniqueHue = 1175;

        [Constructable]
        public BurningGargoyle()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2) // Faster reaction times
        {
            Name = "a Burning Gargoyle";
            Body = 130;                  // Same body as FireGargoyle
            BaseSoundID = 0x174;         // Same sound as FireGargoyle
            Hue = UniqueHue;

            // --- Boosted Stats ---
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(300, 350);

            SetHits(1000, 1200);
            SetStam(250, 300);
            SetMana(300, 350);

            SetDamage(25, 35);

            // --- Damage Types (mostly fire) ---
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 70, 80);
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Advanced Skills ---
            SetSkill(SkillName.EvalInt, 110.1, 125.0);
            SetSkill(SkillName.Magery, 110.1, 125.0);
            SetSkill(SkillName.MagicResist, 120.1, 135.0);
            SetSkill(SkillName.Tactics, 110.1, 120.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 5; // Boss-level creature

            // Initialize ability cooldowns (staggering initial triggers)
            m_NextFireScreamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextEmberStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextWingFlapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_LastLocation = this.Location;

            // --- Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 20)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 10)));
            if (Utility.RandomDouble() < 0.005) // Rare drop chance
            {
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
            }
        }

        public override bool CanFly { get { return true; } }

        // --- Aura & Trail Effect ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && InRange(m.Location, 2))
            {
                // Leave a burning field at the mobile's old location
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 2;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Visual and audio feedback
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // Deal immediate fire damage
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Leave a small flame trail upon movement
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(6 + Utility.Random(3));
                int damage = 2;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Ensure we have a valid combatant and that it is a Mobile before proceeding
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            if (!(Combatant is Mobile target))
                return;

            // --- Ability Triggers ---

            // Fire Scream Attack: a direct, cone-like damage effect at moderate range
            if (DateTime.UtcNow >= m_NextFireScreamTime && this.InRange(target.Location, 6))
            {
                FireScreamAttack(target);
                m_NextFireScreamTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // Ember Storm Attack: an AoE attack affecting targets nearby
            else if (DateTime.UtcNow >= m_NextEmberStormTime && this.InRange(target.Location, 8))
            {
                EmberStormAttack();
                m_NextEmberStormTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            // Wing Flap Attack: close-range knockback/damage ability
            else if (DateTime.UtcNow >= m_NextWingFlapTime && this.InRange(target.Location, 4))
            {
                WingFlapAttack(target);
                m_NextWingFlapTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            }
        }

        // --- Unique Ability: Fire Scream Attack ---
        public void FireScreamAttack(Mobile target)
        {
            if (Map == null || target == null || !target.Alive)
                return;

            this.Say("*The Burning Gargoyle releases a searing scream!*");
            PlaySound(0x208);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 40, UniqueHue, 0, 5052, 0);

            if (target != null && this.InRange(target.Location, 6))
            {
                DoHarmful(target);
                // Primary damage from the scream
                AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 0, 100, 0, 0, 0);

                // Apply additional burning damage after a short delay
                Timer.DelayCall(TimeSpan.FromSeconds(1.0), delegate()
                {
                    if (target != null && target.Alive && this.CanBeHarmful(target, false))
                    {
                        AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    }
                });
            }
        }

        // --- Unique Ability: Ember Storm Attack (Area of Effect) ---
        public void EmberStormAttack()
        {
            if (Map == null)
                return;

            this.Say("*The Burning Gargoyle summons a storm of smoldering embers!*");
            PlaySound(0x5C4);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                0x3709, 15, 50, UniqueHue, 0, 5052, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(20, 35);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
            }
        }

        // --- Unique Ability: Wing Flap Attack (Knockback and Damage) ---
        public void WingFlapAttack(Mobile target)
        {
            if (Map == null || target == null || !target.Alive)
                return;

            this.Say("*The Burning Gargoyle slams its wings with fiery force!*");
            PlaySound(0x20B);
            DoHarmful(target);
            AOS.Damage(target, this, Utility.RandomMinMax(15, 25), 0, 100, 0, 0, 0);

            // (Optional: Implement a positional knockback here if desired)
        }

        // --- Death Explosion Effect ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
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

                if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                {
                    lavaLocation.Z = Map.GetAverageZ(lavaLocation.X, lavaLocation.Y);
                    if (!Map.CanFit(lavaLocation.X, lavaLocation.Y, lavaLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(lavaLocation);

                HotLavaTile droppedLava = new HotLavaTile(); // Assuming HotLavaTile is defined elsewhere
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 3; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 65.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(3, 6));
            if (Utility.RandomDouble() < 0.005)
            {
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
            }
        }

        public BurningGargoyle(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextFireScreamTime);
            writer.Write(m_NextEmberStormTime);
            writer.Write(m_NextWingFlapTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextFireScreamTime = reader.ReadDateTime();
            m_NextEmberStormTime = reader.ReadDateTime();
            m_NextWingFlapTime = reader.ReadDateTime();
            m_LastLocation = this.Location;
        }
    }
}
