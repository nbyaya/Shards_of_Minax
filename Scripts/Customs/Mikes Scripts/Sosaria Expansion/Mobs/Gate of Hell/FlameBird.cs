using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;            // For potential spell effects
using Server.Network;           // For effects and particle calls
using Server.Spells.Fourth;     // For firefield spell (assumed)

namespace Server.Mobiles
{
    [CorpseName("a blazing bird carcass")]
    public class FlameBird : BaseCreature
    {
        // Unique hue for this fire monster (using a vibrant red/orange)
        private const int UniqueHue = 1161;

        // Cooldown timers for the unique abilities
        private DateTime m_NextDiveTime;
        private DateTime m_NextScreechTime;
        private DateTime m_NextFeatherBurstTime;

        // For tracking movement for trail effects
        private Point3D m_LastLocation;

        [Constructable]
        public FlameBird() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Flame Bird";
            Body = 95;               // Using the turkey body as a base
            BaseSoundID = 0x66A;      // Using the turkey’s base sound
            Hue = UniqueHue;         // Unique fiery hue

            // --- Boosted Stats ---
            SetStr(200, 250);
            SetDex(150, 200);
            SetInt(80, 100);

            SetHits(900, 1100);
            SetStam(300, 400);
            SetMana(200, 250);

            SetDamage(15, 20);

            // Damage composition: part physical, mostly fire.
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Fire, 70);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 80, 90);
            // Vulnerable to cold (set low)
            SetResistance(ResistanceType.Cold, 0, 5);
            SetResistance(ResistanceType.Poison, 30, 40);
            SetResistance(ResistanceType.Energy, 30, 40);

            // --- Skills ---
            SetSkill(SkillName.Magery, 80.0, 100.0);
            SetSkill(SkillName.MagicResist, 90.0, 110.0);
            SetSkill(SkillName.Tactics, 80.0, 100.0);
            SetSkill(SkillName.Wrestling, 70.0, 90.0);

            Fame = 8000;
            Karma = -8000;
            VirtualArmor = 50;

            ControlSlots = 3;
            Tamable = true;

            // Initialize ability cooldowns with staggered initial timers
            m_NextDiveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextScreechTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextFeatherBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            m_LastLocation = this.Location;

            // Example loot ingredient
            PackItem(new SulfurousAsh(Utility.RandomMinMax(3, 7)));
        }

        // --- Unique Ability: Blazing Dive ---
        // The Flame Bird swoops on a nearby target, dealing fire damage and leaving a fire field trail.
        public void BlazingDive()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            // Play the base sound and send a visual particle effect at current location.
            PlaySound(BaseSoundID);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 10, 20, UniqueHue, 0, 5029, 0);

            // Deal a burst of fire damage to the target.
            DoHarmful(target);
            AOS.Damage(target, this, Utility.RandomMinMax(30, 50), 0, 100, 0, 0, 0);

            // Leave a brief fire field at the prior location.
            int itemID = 0x398C; // Fire field item id (as in your InfernoElemental)
            TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
            int fieldDamage = 2;
            new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, m_LastLocation, this, this.Map, duration, fieldDamage);
            
            // Update the last location for future trail effects.
            m_LastLocation = this.Location;
        }

        // --- Unique Ability: Flaming Screech ---
        // Emits a fearsome screech causing an AoE burst of fire damage.
        public void FlamingScreech()
        {
            if (Combatant == null || Map == null)
                return;

            // Use a screech sound (example sound id 0x20D – adjust as needed)
            PlaySound(0x20D);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 15, 30, UniqueHue, 0, 5029, 0);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 4);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            // Damage each target with pure fire damage.
            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                int damage = Utility.RandomMinMax(15, 25);
                AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
            }
        }

        // --- Unique Ability: Fire Feather Burst ---
        // A burst of fiery feathers that radiates outward to damage any nearby foes.
        public void FireFeatherBurst()
        {
            if (Map == null)
                return;

            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3728, 20, 30, UniqueHue, 0, 5029, 0);

            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(10, 20);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }
            eable.Free();
        }

        public override void OnThink()
        {
            base.OnThink();

            // --- Trail Effect: Leave a brief fire field along the movement path.
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(3 + Utility.Random(2));
                int fieldDamage = 1;
                new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, m_LastLocation, this, this.Map, duration, fieldDamage);
                m_LastLocation = this.Location;
            }

            // --- Ability Triggers ---
            // Verify Combatant is a Mobile.
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;
            if (!(Combatant is Mobile target))
                return;

            // If the Flame Bird is close enough, consider a Blazing Dive.
            if (DateTime.UtcNow >= m_NextDiveTime && this.InRange(target.Location, 5))
            {
                BlazingDive();
                m_NextDiveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Otherwise, if within range, use a Flaming Screech.
            else if (DateTime.UtcNow >= m_NextScreechTime && this.InRange(target.Location, 6))
            {
                FlamingScreech();
                m_NextScreechTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // And periodically, release a Fire Feather Burst.
            else if (DateTime.UtcNow >= m_NextFeatherBurstTime)
            {
                FireFeatherBurst();
                m_NextFeatherBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }
        }

        // --- Overridden Sound Methods ---
        public override int GetIdleSound()    { return BaseSoundID; }
        public override int GetAngerSound()   { return BaseSoundID; }
        public override int GetHurtSound()    { return BaseSoundID + 1; }
        public override int GetDeathSound()   { return BaseSoundID + 1; }

        // --- Death Explosion ---
        // On death, the Flame Bird causes a fiery explosion and spawns fire field elements.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                int fireBurstCount = 8;
                List<Point3D> effectLocations = new List<Point3D>();
                for (int i = 0; i < fireBurstCount; i++)
                {
                    int xOffset = Utility.RandomMinMax(-2, 2);
                    int yOffset = Utility.RandomMinMax(-2, 2);
                    if (xOffset == 0 && yOffset == 0 && i < fireBurstCount - 1)
                        xOffset = Utility.RandomBool() ? 1 : -1;

                    Point3D flameLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                    if (!Map.CanFit(flameLocation.X, flameLocation.Y, flameLocation.Z, 16, false, false))
                    {
                        flameLocation.Z = Map.GetAverageZ(flameLocation.X, flameLocation.Y);
                        if (!Map.CanFit(flameLocation.X, flameLocation.Y, flameLocation.Z, 16, false, false))
                            continue;
                    }

                    effectLocations.Add(flameLocation);

                    // Spawn a fire field element at the location.
                    HotLavaTile droppedLava = new HotLavaTile();
                    droppedLava.Hue = UniqueHue;
                    droppedLava.MoveToWorld(flameLocation, this.Map);

                    Effects.SendLocationParticles(EffectItem.Create(flameLocation, this.Map, EffectItem.DefaultDuration),
                                                  0x3709, 10, 20, UniqueHue, 0, 5016, 0);
                }

                Point3D deathLocation = this.Location;
                if (effectLocations.Count > 0)
                    deathLocation = effectLocations[Utility.Random(effectLocations.Count)];
                Effects.PlaySound(deathLocation, this.Map, 0x218);
                Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                                              0x3709, 10, 40, UniqueHue, 0, 5052, 0);
            }

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.MedScrolls, 1);

        }

        public FlameBird(Serial serial) : base(serial)
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

            m_NextDiveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextScreechTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextFeatherBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
        }
    }
}
