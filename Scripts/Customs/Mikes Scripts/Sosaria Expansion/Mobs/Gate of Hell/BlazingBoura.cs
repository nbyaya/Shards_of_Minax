using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fourth; // For firefield effects

namespace Server.Mobiles
{
    [CorpseName("a blazing boura corpse")]
    public class BlazingBoura : BaseCreature, ICarvable
    {
        // Timers to prevent ability spamming
        private DateTime m_NextNovaTime;
        private DateTime m_NextChargeTime;
        // Track the creature’s last location for movement effects
        private Point3D m_LastLocation;
        // For carving (e.g. recovering its special charred hide)
        private bool m_GatheredFur;

        // Unique hue for the fire effect (here we reuse 1161; adjust as desired)
        private const int UniqueHue = 1161;

        [Constructable]
        public BlazingBoura() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a blazing boura";
            Body = 715; // Based on the HighPlainsBoura body
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(500, 550);
            SetDex(120, 140);
            SetInt(150, 170);

            SetHits(1000, 1200);
            SetStam(120, 140);
            SetMana(150, 170);

            SetDamage(25, 30);
            // Mostly fire damage with a slight physical component
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 80, 90);
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to Cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 40, 50);

            // --- Skills (advanced mage/beast hybrid) ---
            SetSkill(SkillName.EvalInt, 100.0, 110.0);
            SetSkill(SkillName.Magery, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 100.0, 120.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 60;
            ControlSlots = 4;
            Tamable = true;

            // --- Ability cooldown initialization ---
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
            m_LastLocation = this.Location;

            // --- Initial loot (fire-based ingredients) ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));
        }

        public BlazingBoura(Serial serial) : base(serial)
        {
        }

		public static class DirectionHelper
		{
			public static Point2D GetOffset(Direction d)
			{
				switch (d)
				{
					case Direction.North: return new Point2D(0, -1);
					case Direction.Right: return new Point2D(1, -1);
					case Direction.East: return new Point2D(1, 0);
					case Direction.Down: return new Point2D(1, 1);
					case Direction.South: return new Point2D(0, 1);
					case Direction.Left: return new Point2D(-1, 1);
					case Direction.West: return new Point2D(-1, 0);
					case Direction.Up: return new Point2D(-1, -1);
					default: return new Point2D(0, 0);
				}
			}
		}


        #region Unique Abilities

        // --- Movement-based Flame Trail ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && m.InRange(this.Location, 2))
            {
                // Leave behind a short-lived fire field at the mobile's previous location
                int itemID = 0x398C; // Fire field graphic (adjust as needed)
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                int damage = 2;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Fire hit effect on nearby creature
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208); // Fire explosion sound
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    AOS.Damage(m, this, Utility.RandomMinMax(4, 8), 0, 100, 0, 0, 0); // Damage is fire-only
            }
            base.OnMovement(m, oldLocation);

            // Additionally, if the Blazing Boura itself has moved, leave a short trail effect at its last location
            if (this.Location != m_LastLocation)
            {
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(4 + Utility.Random(2));
                int damage = 2;
                var trailField = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, m_LastLocation, this, this.Map, duration, damage);
                m_LastLocation = this.Location;
            }
        }

        // --- Periodic Ability Logic ---
        public override void OnThink()
        {
            base.OnThink();

            // Ensure we have a valid combatant and map before checking abilities
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Only access Mobile-specific members if Combatant is a Mobile
            if (Combatant is Mobile target)
            {
                // --- Flame Nova Attack: Wide-area burst ---
                if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(target.Location, 6))
                {
                    FlameNovaAttack();
                    m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
                }
                // --- Blaze Charge Attack: A fiery charging strike ---
                else if (DateTime.UtcNow >= m_NextChargeTime && this.InRange(target.Location, 4))
                {
                    BlazeChargeAttack(target);
                    m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(16, 24));
                }
            }
        }

        // --- Ability 1: Flame Nova ---
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            // Play sound and self-effects
            this.PlaySound(0x208);
            this.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                // Send a visual explosion effect
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile m in targets)
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0); // Pure fire damage
                    m.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Ability 2: Blaze Charge ---
        public void BlazeChargeAttack(Mobile target)
        {
            if (Map == null || target == null || !CanBeHarmful(target))
                return;

            // Announce the charge with optional flavor text and sound
            this.Say("*The Blazing Boura charges with flames ablaze!*");
            PlaySound(0x160); // Charge or impact sound

            // Calculate the direction toward the target and charge distance (e.g. 3 tiles forward)
            int chargeDistance = 3;
            Direction dir = GetDirectionTo(target);
            Point3D start = this.Location;
            List<Point3D> chargePath = new List<Point3D>();

            // Build the line of tiles in front of the creature along its direction
            for (int i = 1; i <= chargeDistance; ++i)
            {
				Point2D offset = DirectionHelper.GetOffset(dir);
				Point3D nextPoint = new Point3D(this.X + (offset.X * i), this.Y + (offset.Y * i), this.Z);
                if (Map.CanFit(nextPoint.X, nextPoint.Y, nextPoint.Z, 16, false, false))
                    chargePath.Add(nextPoint);
                else
                    break; // Stop if blocked
            }

            // For every tile along the charge, apply damage to any mobiles nearby
            foreach (Point3D loc in chargePath)
            {
                IPooledEnumerable eable = Map.GetMobilesInRange(loc, 1);
                foreach (Mobile m in eable)
                {
                    if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    {
                        DoHarmful(m);
                        int damage = Utility.RandomMinMax(50, 70);
                        AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        m.FixedParticles(0x3709, 10, 25, 5032, EffectLayer.Head);
                    }
                }
                eable.Free();

                // Send particles along the charge path for visual flare
                Effects.SendLocationParticles(EffectItem.Create(loc, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5029, 0);
            }

            // Optionally, teleport the creature to the end of its charge path if space allows
            if (chargePath.Count > 0)
                this.Location = chargePath[chargePath.Count - 1];
        }

        #endregion

        #region Carving & Resources

        // --- ICarvable Implementation ---
        public bool Carve(Mobile from, Item item)
        {
            if (!m_GatheredFur)
            {
                // Attempt to drop the charred hide into the player's backpack
                Item fur = new MaxxiaScroll(); // Assume you have defined this custom item elsewhere
                if (from.Backpack == null || !from.Backpack.TryDropItem(from, fur, false))
                {
                    from.SendLocalizedMessage(1112352); // "You would not be able to place the gathered fur in your backpack!"
                    fur.Delete();
                }
                else
                {
                    from.SendLocalizedMessage(1112353); // "You place the gathered fur into your backpack."
                    m_GatheredFur = true;
                    return true;
                }
            }
            else
            {
                PrivateOverheadMessage(MessageType.Regular, 0x3B2, 1112354, from.NetState); // "The creature glares at you and will not let you carve any more."
            }

            return false;
        }

        public override int Meat { get { return 10; } }
        public override int Hides { get { return 25; } }
        public override int DragonBlood { get { return 10; } }
        public override HideType HideType { get { return HideType.Horned; } }
        public override FoodType FavoriteFood { get { return FoodType.FruitsAndVegies; } }

        #endregion

        #region Sounds

        public override int GetIdleSound()
        {
            return 1507; // As per original HighPlainsBoura
        }

        public override int GetAngerSound()
        {
            return 1504;
        }

        public override int GetHurtSound()
        {
            return 1506;
        }

        public override int GetDeathSound()
        {
            return 1505;
        }

        #endregion

        #region Death Explosion

        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 10;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);

                // Ensure we don't repeatedly pick the exact death spot
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

                // Spawn the fire tile (assumes HotLavaTile is defined elsewhere)
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                // Small explosion effect at each tile
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Central explosion effect at one of the generated locations
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        #endregion

        #region Loot Generation & Serialization

        public override void GenerateLoot()
        {
            // Advanced loot packages
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Chance for a unique drop
            if (Utility.RandomDouble() < 0.01)
                PackItem(new InfernosEmbraceCloak()); // Assumes you have defined this unique item elsewhere

            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5))); // Example rare resource
        }

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_GatheredFur);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_GatheredFur = reader.ReadBool();

            // Reinitialize ability cooldowns upon loading
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 16));
        }

        #endregion
    }
}
