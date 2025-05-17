using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;           // For spell effects
using Server.Network;          // For particle and sound effects
using System.Collections.Generic;
using Server.Spells.Fourth;    // For fire field effects

namespace Server.Mobiles
{
    [CorpseName("a charred skeletal corpse")]
    public class ScorchedLich : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextCinderNovaTime;
        private DateTime m_NextMeteorTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // Unique fiery hue for the Scorched Lich
        private const int UniqueHue = 1356; // Adjust as desired

        [Constructable]
        public ScorchedLich() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Scorched Lich";
            Body = 309;             // Same skeletal body as base Lich
            BaseSoundID = 0x48D;      // Same skeletal sound
            Hue = UniqueHue;

            // --- Advanced Stats ---
            SetStr(400, 500);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(1500, 1800);
            SetStam(200, 250);
            SetMana(400, 500);

            SetDamage(25, 35);

            // --- Damage Types: Mostly Fire with a touch of Physical ---
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, -5, 5);     // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 40, 50);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Skills: Combining Magery & Necromancy ---
            SetSkill(SkillName.EvalInt, 110.0, 130.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.Necromancy, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 125.0, 140.0);
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 5;   // Boss-level creature

            // --- Initialize ability cooldown timers ---
            m_NextCinderNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation = this.Location;

            // --- Pack some thematic loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));
            AddItem(new LightSource());  // Retain a light effect
        }

        // --- Fire Aura: Leave burning trails when mobiles move near the lich ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                // Leave a burning field at the mobile's old location.
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 2;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Fire hit visual and sound effect.
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // Damage the mobile (fire damage only)
                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(8, 12), 0, 100, 0, 0, 0);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Main thinking routine for special attacks ---
        public override void OnThink()
        {
            base.OnThink();

            // Create a burning trail effect if the lich has moved.
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 2;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Ensure we have a valid combatant.
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Use "if (Combatant is Mobile target)" before using Mobile-specific properties.
            if (Combatant is Mobile target)
            {
                // --- Cinder Nova Attack: AoE burst (range 6) ---
                if (DateTime.UtcNow >= m_NextCinderNovaTime && this.InRange(target.Location, 6))
                {
                    CinderNovaAttack();
                    m_NextCinderNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
                }
                // --- Meteor Swarm Attack: Incendiary meteors (range 12) ---
                else if (DateTime.UtcNow >= m_NextMeteorTime && this.InRange(target.Location, 12))
                {
                    MeteorSwarmAttack();
                    m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
                }
                // --- Summon Fire Wraiths: Call in allies (range 10) ---
                else if (DateTime.UtcNow >= m_NextSummonTime && this.InRange(target.Location, 10))
                {
                    SummonFireWraiths();
                    m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
                }
            }
        }

        // --- Unique Ability: Cinder Nova Attack ---
        public void CinderNovaAttack()
        {
            if (Map == null)
                return;

            // Play a distinctive sound and visual effect.
            this.PlaySound(0x208); // Fiery explosion sound
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
                // Display a radial fire burst effect.
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(50, 70); // Significant AoE fire damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

                    // Additional visual hit effect.
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- Unique Ability: Meteor Swarm Attack ---
        public void MeteorSwarmAttack()
        {
            if (Combatant == null || Map == null)
                return;

            if (!(Combatant is Mobile target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Scorching meteors rain down!*");
            PlaySound(0x160); // Meteor impact sound

            int meteorCount = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < meteorCount; i++)
            {
                // Slightly randomize impact positions around the target.
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z);

                // Validate impact point on the map.
                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Start the meteor high above.
                Point3D startPoint = new Point3D(impactPoint.X + Utility.RandomMinMax(-1, 1),
                                                   impactPoint.Y + Utility.RandomMinMax(-1, 1),
                                                   impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map),
                    new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay damage to sync with the visual impact.
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

        // --- Unique Ability: Summon Fire Wraiths ---
        public void SummonFireWraiths()
        {
            // Summon 2-3 fire wraiths near the Scorched Lich as temporary allies.
            int count = Utility.RandomMinMax(2, 3);
            for (int i = 0; i < count; i++)
            {
                // Determine a nearby spawn point.
                Point3D spawnLocation = new Point3D(
                    this.X + Utility.RandomMinMax(-2, 2),
                    this.Y + Utility.RandomMinMax(-2, 2),
                    this.Z);

                // Validate the spawn location.
                if (!Map.CanFit(spawnLocation.X, spawnLocation.Y, spawnLocation.Z, 16, false, false))
                    spawnLocation.Z = Map.GetAverageZ(spawnLocation.X, spawnLocation.Y);

                // Assume FireWraith is defined elsewhere on your shard.
                ScaldingWraith wraith = new ScaldingWraith();
                wraith.Hue = UniqueHue;
                wraith.MoveToWorld(spawnLocation, this.Map);
            }

            this.Say("*I summon the wrath of flame incarnate!*");
            PlaySound(0x20D); // A summoning sound effect
        }

        // --- Death Explosion: Leave behind burning lava tiles ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 15; // More lava tiles for a grand effect
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < lavaTilesToDrop; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);

                // Avoid the exact center most of the time.
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

                // Spawn HotLavaTile (assumed defined elsewhere)
                HotLavaTile droppedLava = new HotLavaTile();
                droppedLava.Hue = UniqueHue;
                droppedLava.MoveToWorld(lavaLocation, this.Map);

                // Create a quick flame effect at each lava tile.
                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Central explosion effect
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218); // Large explosion sound
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override int TreasureMapLevel { get { return 6; } }

        // Increased dispel difficulty for bosses.
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

        }

        // --- Serialization ---
        public ScorchedLich(Serial serial) : base(serial)
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

            // Reinitialize ability cooldowns on load.
            m_NextCinderNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
