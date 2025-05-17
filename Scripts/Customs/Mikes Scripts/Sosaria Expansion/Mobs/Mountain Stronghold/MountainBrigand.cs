using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For potential spell effects
using Server.Network; // For visual and sound effects
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a mountain brigand corpse")]
    public class MountainBrigand : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextRockslideTime;
        private DateTime m_NextBoulderTime;
        private DateTime m_NextFissureTime;

        // Unique hue to tie into the mountain/rock theme
        private const int UniqueHue = 1170;

        [Constructable]
        public MountainBrigand() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Title = "the mountain brigand";
            Hue = UniqueHue;

            // Replicate base brigand's gender selection and appearance
            if (Female = Utility.RandomBool())
            {
                Body = 0x191;
                Name = NameList.RandomName("female");
                AddItem(new Skirt(Utility.RandomNeutralHue()));
            }
            else
            {
                Body = 0x190;
                Name = NameList.RandomName("male");
                AddItem(new ShortPants(Utility.RandomNeutralHue()));
            }

            // Enhanced stats for an advanced monster
            SetStr(200, 250);
            SetDex(150, 180);
            SetInt(100, 130);

            SetHits(1200, 1400);
            SetStam(300, 350);
            SetMana(100, 150); // Some mana for abilities

            SetDamage(20, 30);
            // Damage types: mostly physical with a hint of energy
            SetDamageType(ResistanceType.Physical, 80);
            SetDamageType(ResistanceType.Energy, 20);

            // Resistances – boosted to represent a hardened mountain denizen
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills – improved melee and tactical abilities
            SetSkill(SkillName.Fencing, 80.0, 100.0);
            SetSkill(SkillName.Macing, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 50.0, 70.0);
            SetSkill(SkillName.Swords, 80.0, 100.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 50.0, 70.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Equip basic clothing and accessories with a mountain twist
            AddItem(new Boots(Utility.RandomNeutralHue()));
            AddItem(new FancyShirt());
            AddItem(new Bandana());

            // Randomly assign a weapon (as per the base brigand)
            switch (Utility.Random(7))
            {
                case 0: AddItem(new Longsword()); break;
                case 1: AddItem(new Cutlass()); break;
                case 2: AddItem(new Broadsword()); break;
                case 3: AddItem(new Axe()); break;
                case 4: AddItem(new Club()); break;
                case 5: AddItem(new Dagger()); break;
                case 6: AddItem(new Spear()); break;
            }

            Utility.AssignRandomHair(this);

            // Initialize ability cooldown timers
            m_NextRockslideTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextFissureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }

        public MountainBrigand(Serial serial) : base(serial)
        {
        }

        public override bool ClickTitle { get { return false; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }

        // When this monster dies, it creates an avalanche effect
        public override void OnDeath(Container c)
        {
            base.OnDeath(c);

            if (Map != null)
            {
                this.Say("*The mountain claims its due!*");
                Effects.PlaySound(Location, Map, 0x20F); // Avalanche or grinding rock sound
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 15, 50, UniqueHue, 0, 5052, 0);

                // Drop a few EarthquakeTiles to simulate falling rocks
                int hazardsToDrop = Utility.RandomMinMax(3, 6);
                for (int i = 0; i < hazardsToDrop; i++)
                {
                    int xOffset = Utility.RandomMinMax(-3, 3);
                    int yOffset = Utility.RandomMinMax(-3, 3);
                    Point3D hazardLocation = new Point3D(X + xOffset, Y + yOffset, Z);

                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                    {
                        hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                        if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                            continue;
                    }

                    EarthquakeTile tile = new EarthquakeTile(); // Assumes EarthquakeTile is defined elsewhere
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(hazardLocation, Map);
                }
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Average);

            // A chance for a unique, mountain-themed drop
            if (Utility.RandomDouble() < 0.05)
            {
                // Example: Pack a rare weapon or artifact (replace MountainHeart with your custom item if desired)
                PackItem(new Longsword()); // Placeholder for a unique drop
            }
        }

        // -----------------------------------------------------
        // Unique Abilities: Movement and Special Attacks
        // -----------------------------------------------------

        // Movement Effect: When enemies come too close, the Mountain Brigand drains their stamina.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && Alive && InRange(m.Location, 2) && CanBeHarmful(m, false))
            {
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int stamDrain = Utility.RandomMinMax(10, 15);
                    if (target.Stam >= stamDrain)
                    {
                        target.Stam -= stamDrain;
                        target.SendMessage(0x22, "The crushing weight of the mountain saps your strength!");
                        target.FixedParticles(0x376A, 10, 15, 5013, UniqueHue, 0, EffectLayer.Head);
                        target.PlaySound(0x1E0); // Grinding rock sound
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Use unique abilities based on distance and cooldown:
            // Boulder Toss: When the target is at moderate range (8-15 tiles)
            if (DateTime.UtcNow >= m_NextBoulderTime && InRange(Combatant.Location, 15) && !InRange(Combatant.Location, 8))
            {
                BoulderTossAttack();
                m_NextBoulderTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            // Rockslide Attack: When the target is very close (within 5 tiles)
            else if (DateTime.UtcNow >= m_NextRockslideTime && InRange(Combatant.Location, 5))
            {
                RockslideAttack();
                m_NextRockslideTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Fissure Strike Attack: When the target is within a moderate range (within 10 tiles)
            else if (DateTime.UtcNow >= m_NextFissureTime && InRange(Combatant.Location, 10))
            {
                FissureStrikeAttack();
                m_NextFissureTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // ------------------------------------------------------------------------
        // Rockslide Attack: An AoE ability that emulates falling rocks damaging foes.
        public void RockslideAttack()
        {
            if (Map == null) return;

            this.Say("*Feel the mountain's wrath!*");
            PlaySound(0x20E); // Rock sliding sound
            FixedParticles(0x3709, 10, 30, 5021, UniqueHue, 0, EffectLayer.CenterFeet);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x376A, 15, 40, UniqueHue, 0, 5039, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(25, 40);
                    // Physical damage type (100% physical)
                    AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                    // Leave a lingering hazard using an EarthquakeTile
                    if (Map.CanFit(target.X, target.Y, target.Z, 16, false, false))
                    {
                        EarthquakeTile tile = new EarthquakeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(target.X, target.Y, target.Z), Map);
                    }
                }
            }
        }

        // ------------------------------------------------------------------------
        // Boulder Toss Attack: A ranged projectile attack mimicking a massive boulder thrown from above.
        public void BoulderTossAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable combatantDamageable = Combatant;
            if (combatantDamageable is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*Take this, from the heights!*");
                PlaySound(0x20A); // Throwing sound

                // Create the projectile effect from this creature to the target.
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, Location, Map),
                    new Entity(Serial.Zero, target.Location, Map),
                    0x36D4, // Graphic representing a boulder
                    10, 0, false, false, UniqueHue, 0, 9501, 1, 0, EffectLayer.Waist, 0x100);

                // Delay to simulate travel time before impact
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (target != null && CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        int damage = Utility.RandomMinMax(30, 50);
                        // Apply physical damage
                        AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);
                        // Optionally: add a brief stun or knockback effect here for flavor.
                    }
                });
            }
        }

        // ------------------------------------------------------------------------
        // Fissure Strike Attack: A targeted ground attack that creates a fissure hazard at the enemy's location.
        public void FissureStrikeAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;
            if (targetDamageable is Mobile target && CanBeHarmful(target, false))
                targetLocation = target.Location;
            else
                targetLocation = targetDamageable.Location;

            this.Say("*The earth splits beneath you!*");
            PlaySound(0x1F2); // Ground cracking sound
            Effects.SendLocationParticles(EffectItem.Create(targetLocation, Map, EffectItem.DefaultDuration),
                0x3728, 10, 10, UniqueHue, 0, 5039, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Map == null)
                    return;

                Point3D spawnLoc = targetLocation;
                if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                {
                    spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                        return;
                }

                // Use a LandmineTile to simulate a fissure hazard on the ground.
                LandmineTile fissureTile = new LandmineTile();
                fissureTile.Hue = UniqueHue;
                fissureTile.MoveToWorld(spawnLoc, Map);
            });
        }

        // ------------------------------------------------------------------------
        // Serialization for persistence

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            writer.Write(m_NextRockslideTime);
            writer.Write(m_NextBoulderTime);
            writer.Write(m_NextFissureTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextRockslideTime = reader.ReadDateTime();
            m_NextBoulderTime = reader.ReadDateTime();
            m_NextFissureTime = reader.ReadDateTime();
        }
    }
}
