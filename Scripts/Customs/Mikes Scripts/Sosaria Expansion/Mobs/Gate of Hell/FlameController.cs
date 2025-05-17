using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;         // For spell-related effects
using Server.Network;        // For effects and sound
using System.Collections.Generic;
using Server.Spells.Fourth;   // For firefield effects

namespace Server.Mobiles
{
    [CorpseName("an incinerated controller corpse")]
    public class FlameController : BaseCreature
    {
        // Cooldown timers for special abilities
        private DateTime m_NextNovaTime;
        private DateTime m_NextMeteorTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // Unique hue for the fire-themed monster
        private const int UniqueHue = 1161;

        [Constructable]
        public FlameController() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Flame Controller";
            Title = "the controller";
            Body = 400; // Same as the original Golem Controller body
            Hue = UniqueHue;
            BaseSoundID = 0x208; // Using a fire/explosion-like sound

            // --- Significantly Boosted Stats ---
            SetStr(450, 550);
            SetDex(210, 260);
            SetInt(380, 450);

            SetHits(1200, 1500);
            SetStam(210, 260);
            SetMana(380, 450);

            SetDamage(22, 28);
            // Damage is 10% physical, 90% fire
            SetDamageType(ResistanceType.Physical, 10);
            SetDamageType(ResistanceType.Fire, 90);

            // --- Adjusted Resistances ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 85, 95);
            SetResistance(ResistanceType.Cold, -10, 0); // Vulnerable to cold
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 115.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);
            SetSkill(SkillName.Anatomy, 80.0, 90.0);

            Fame = 18000;
            Karma = -18000;

            VirtualArmor = 75;
            ControlSlots = 5;

            // Initialize ability cooldown timers with staggered starting delays
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            // Standard loot plus a chance for extra fire-based ingredients
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));

            // Retain a light source so the creature glows with its fiery nature
            AddItem(new LightSource());

            m_LastLocation = this.Location;
        }

        // --- Passive Ability: Flame Aura on Movement ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive)
            {
                // If any mobile moves within 2 tiles, leave a fire field at its old location.
                if (m.InRange(this.Location, 2))
                {
                    int itemID = 0x398C; // Graphic for a fire field
                    TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                    int damage = 2;

                    var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                    // Trigger visual and sound effects (flamestrike style)
                    m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                    m.PlaySound(0x208);

                    // Damage the mobile (fire damage only)
                    if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    {
                        DoHarmful(m);
                        AOS.Damage(m, this, Utility.RandomMinMax(8, 15), 0, 100, 0, 0, 0);
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Override OnThink to trigger special attacks ---
        public override void OnThink()
        {
            base.OnThink();

            // Create flame trails when the controller moves
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 2;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Ensure we have a valid combatant
            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Check that Combatant is a Mobile before accessing Mobile-specific properties
            if (!(Combatant is Mobile target))
                return;

            // --- Flame Nova Attack ---
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(target.Location, 8))
            {
                FlameNovaAttack();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
            // --- Meteor Swarm Attack ---
            else if (DateTime.UtcNow >= m_NextMeteorTime && this.InRange(target.Location, 12))
            {
                MeteorSwarmAttack();
                m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // --- Summon Fire Imps Attack ---
            else if (DateTime.UtcNow >= m_NextSummonTime)
            {
                SummonFireImpsAttack();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }
        }

        // --- Unique Ability: Flame Nova Attack ---
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            // Play a roaring fire sound and create a burst effect on self
            this.PlaySound(0x208);
            this.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6); // 6 tile AoE

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

        // --- Unique Ability: Meteor Swarm Attack ---
        public void MeteorSwarmAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Incinerating rain!*");
            PlaySound(0x160); // Meteor crash sound

            int meteorCount = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < meteorCount; i++)
            {
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z
                );

                // Validate the impact point location on the map
                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                Point3D startPoint = new Point3D(
                    impactPoint.X + Utility.RandomMinMax(-1, 1),
                    impactPoint.Y + Utility.RandomMinMax(-1, 1),
                    impactPoint.Z + 30);

                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map),
                                              new Entity(Serial.Zero, impactPoint, this.Map),
                                              0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

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

        // --- Unique Ability: Summon Fire Imps ---
        // This ability spawns a few minor fire creatures to assist in battle.
        public void SummonFireImpsAttack()
        {
            // Optional: Adjust the number and type of summoned creatures as needed.
            int impsToSummon = Utility.RandomMinMax(2, 3);
            for (int i = 0; i < impsToSummon; i++)
            {
                // Create a new fire imp creature (assumes FireImp is defined elsewhere)
                BaseCreature fireImp = new FireImp();
                // Position them adjacent to the FlameController
                Point3D spawnPoint = new Point3D(
                    this.X + Utility.RandomMinMax(-1, 1),
                    this.Y + Utility.RandomMinMax(-1, 1),
                    this.Z
                );

                // Ensure the spawn point is valid
                if (!Map.CanFit(spawnPoint.X, spawnPoint.Y, spawnPoint.Z, 16, false, false))
                    spawnPoint = this.Location;

                fireImp.MoveToWorld(spawnPoint, this.Map);
                // Optionally, set their Combatant to this FlameController’s target
                if (Combatant is Mobile target)
                    fireImp.Combatant = target;
            }

            // Play a summoning sound for flavor
            PlaySound(0x226);
            this.Say("*I call upon the fires of hell!*");
        }

        // --- Death Explosion: Spawn Lava Tiles ---
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

                // Spawn the HotLavaTile (assumes HotLavaTile is defined elsewhere)
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

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 135.0; } }
        public override double DispelFocus { get { return 60.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Special chance for a unique drop
            if (Utility.RandomDouble() < 0.005)
                PackItem(new MaxxiaScroll());
        }

        // --- Serialization ---
        public FlameController(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0); // version
            // (If needed, serialize timer values here)
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
