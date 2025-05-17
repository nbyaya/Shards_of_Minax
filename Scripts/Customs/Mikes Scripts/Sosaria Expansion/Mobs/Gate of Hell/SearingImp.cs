using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;
using Server.Network;
using Server.Spells.Fourth; // For FireField effects and similar

namespace Server.Mobiles
{
    [CorpseName("a smoldering imp corpse")]
    public class SearingImp : BaseCreature
    {
        // Cooldowns for special abilities
        private DateTime m_NextFlameNovaTime;
        private DateTime m_NextMeteorTime;
        private DateTime m_NextChainsTime;
        private Point3D m_LastLocation;

        // Unique hue for our advanced imp (adjust as desired)
        private const int UniqueHue = 0x490; 

        [Constructable]
        public SearingImp()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4) // mage-type for ranged fiery attacks
        {
            Name = "a searing imp";
            Body = 74; // same small imp body as DeadlyImp
            BaseSoundID = 422;
            Hue = UniqueHue;

            // --- Enhanced Stats ---
            SetStr(200, 220);
            SetDex(150, 170);
            SetInt(160, 180);

            SetHits(1800, 2000);
            SetStam(200, 250);
            SetMana(160, 180);

            SetDamage(70, 100);

            // --- Damage Types: 100% fire ---
            SetDamageType(ResistanceType.Physical, 0);
            SetDamageType(ResistanceType.Fire, 100);

            // --- Resistances (fireproof, but vulnerable to cold) ---
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 100, 100); // completely fireproof
            SetResistance(ResistanceType.Cold, -10, 0);    // vulnerable to cold attacks
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 70, 80);

            // --- Enhanced Skills ---
            SetSkill(SkillName.Magery, 120.1, 140.0);
            SetSkill(SkillName.Tactics, 120.1, 140.0);
            SetSkill(SkillName.Wrestling, 100.1, 120.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 80;
            ControlSlots = 5;

            // Initialize ability cooldowns (in UTC)
            m_NextFlameNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));

            // Light up the area with a faint fiery glow (if your shard supports light sources)
            AddItem(new LightSource());

            m_LastLocation = this.Location;
        }

        public SearingImp(Serial serial)
            : base(serial)
        {
        }

        // --- Unique Ability: Flame Aura ---
        // Leaves behind a searing trail damaging nearby foes when they move close.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m == null || m == this || m.Map != this.Map || !m.Alive || !this.Alive)
            {
                base.OnMovement(m, oldLocation);
                return;
            }

            if (m.InRange(this.Location, 2))
            {
                // Create a brief fire field at the mobile's previous location
                int itemID = 0x398C; // Fire field graphic
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int fieldDamage = 2;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, fieldDamage);

                // Visual and sound effects
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // Damage the mobile (fire only)
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(8, 15), 0, 100, 0, 0, 0);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // If the creature has moved, leave a lingering fire field at its last location.
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int fieldDamage = 2;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, fieldDamage);
            }

            // Make sure Combatant is a Mobile before using its properties.
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            if (!(Combatant is Mobile target))
                return;

            // --- Flame Nova Attack: AoE burst ---
            if (DateTime.UtcNow >= m_NextFlameNovaTime && this.InRange(target.Location, 6))
            {
                FlameNovaAttack();
                m_NextFlameNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            // --- Meteor Swarm Attack: Concussive fire meteors ---
            else if (DateTime.UtcNow >= m_NextMeteorTime && this.InRange(target.Location, 12))
            {
                MeteorSwarmAttack();
                m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // --- Infernal Chains Attack: Bind the target with burning chains ---
            else if (DateTime.UtcNow >= m_NextChainsTime && this.InRange(target.Location, 4))
            {
                InfernalChainsAttack();
                m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 18));
            }
        }

        // --- Unique Ability: Flame Nova ---
        // Emits a radial burst of fire damaging all nearby targets.
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x208); // Fiery explosion sound
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot); // Self explosion effect

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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Meteor Swarm ---
        // Calls down a barrage of fiery meteors near the current target.
        public void MeteorSwarmAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Incinerating rain!*");
            PlaySound(0x160); // Meteor impact sound

            int meteorCount = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < meteorCount; i++)
            {
                // Randomize impact points around the target
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z);

                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Launch meteor visual (using a Flamestrike-style effect)
                Point3D startPoint = new Point3D(impactPoint.X + Utility.RandomMinMax(-1, 1), impactPoint.Y + Utility.RandomMinMax(-1, 1), impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map),
                    new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay damage to simulate impact timing
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (this.Map == null)
                        return;

                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration),
                        0x3709, 10, 30, UniqueHue, 0, 2023, 0);
                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(25, 35);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Unique Ability: Infernal Chains ---
        // Binds the target with hellish chains that damage and slow them.
        public void InfernalChainsAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            this.Say("*Chains of burning agony!*");
            // Visual effect: a chain effect from SearingImp to the target
            Effects.SendMovingParticles(new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, target.Location, this.Map),
                0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

            DoHarmful(target);
            int chainDamage = Utility.RandomMinMax(30, 40);
            AOS.Damage(target, this, chainDamage, 0, 100, 0, 0, 0);

            // Attempt to slow the target; using Freeze as a placeholder.
            // Replace target.Freeze with your shard’s appropriate slowing effect.
            target.Freeze(TimeSpan.FromSeconds(2));
        }

        // --- Death Explosion: Hellfire Burst ---
        // Upon death, spawns multiple burning lava tiles around the corpse.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                int lavaTilesToDrop = 12; 
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

                    // Spawn a HotLavaTile at this location (assumes HotLavaTile is defined elsewhere)
                    HotLavaTile droppedLava = new HotLavaTile();
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
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Chance to drop a unique fiery resource
            if (Utility.RandomDouble() < 0.01)
            {
                PackItem(new MaxxiaScroll()); // Ensure ScorchedCore is defined as a unique item elsewhere
            }
        }

        // Searing Imps are immune to bleeding
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        // Increased dispel difficulty
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.WriteEncodedInt(0); // version
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadEncodedInt();

            // Re-initialize ability cooldowns on load
            m_NextFlameNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextChainsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
        }
    }
}
