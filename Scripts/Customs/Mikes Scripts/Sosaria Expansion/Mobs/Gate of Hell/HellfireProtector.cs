using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;           // For possible spell effects
using Server.Network;          // For visual effects/sounds
using System.Collections.Generic; // For list-based AoE targeting
using Server.Spells.Fourth;    // For FireField functionality

namespace Server.Mobiles
{
    [CorpseName("the scorched corpse of the Hellfire Protector")]
    public class HellfireProtector : BaseCreature
    {
        // Timers to manage special abilities
        private DateTime m_NextNovaTime;
        private DateTime m_NextMeteorTime;
        private DateTime m_NextSearingOverloadTime;  // New ability timer
        private Point3D m_LastLocation;

        // Unique Hue for this monster (a blazing fire-red/orange)
        private const int UniqueHue = 1350;

        [Constructable]
        public HellfireProtector()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Hellfire Protector";
            Body = 401;          // Use the base body from the original Protector
            BaseSoundID = 838;   // Use the familiar sound base (adjust if desired)
            Female = true;
            Hue = UniqueHue;
            HairItemID = Race.Human.RandomHair(this);
            HairHue = Race.Human.RandomHairHue();
            Title = "the Hellfire Protector";

            // --- Enhanced Stats ---
            SetStr(900, 1000);
            SetDex(250, 300);
            SetInt(400, 500);
            SetHits(1800, 2200);
            SetStam(300, 350);
            SetMana(400, 500);
            SetDamage(25, 35);

            // --- Damage Types (predominantly fire) ---
            SetDamageType(ResistanceType.Physical, 20);
            SetDamageType(ResistanceType.Fire, 80);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, -5, 5); // Vulnerable to Cold
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 60, 70);

            // --- Skills ---
            SetSkill(SkillName.EvalInt, 110.0, 130.0);
            SetSkill(SkillName.Magery, 110.0, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 80;
            ControlSlots = 5;

            // --- Ability Timer Initialization ---
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSearingOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_LastLocation = this.Location;

            // --- Equip Fiery Gear ---
            Item boots = new ThighBoots();
            boots.Movable = false;
            boots.Hue = UniqueHue;
            AddItem(boots);

            Item shroud = new Item(0x204E);
            shroud.Layer = Layer.OuterTorso;
            shroud.Movable = false;
            shroud.Hue = UniqueHue;
            AddItem(shroud);

            // Add some starting loot ingredients
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(3, 6)));
        }

        public HellfireProtector(Serial serial)
            : base(serial)
        {
        }

        public override bool AlwaysMurderer { get { return true; } }
        public override bool PropertyTitle { get { return false; } }
        public override bool ShowFameTitle { get { return false; } }

        // --- Special Ability: Flame Aura (On Movement) ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(4));
                int damage = 3;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Play a fire effect and sound
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // If m is a Mobile, apply fire damage
                if (m is Mobile target && CanBeHarmful(target, false) && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Main Think Loop: Check for Special Attacks ---
        public override void OnThink()
        {
            base.OnThink();

            // Leave a fire field at the last recorded location if moved
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(5));
                int damage = 3;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // Flame Nova if enemy in range (8 tiles)
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(Combatant.Location, 8))
            {
                FlameNovaAttack();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
            }
            // Meteor Swarm if enemy in range (12 tiles)
            else if (DateTime.UtcNow >= m_NextMeteorTime && this.InRange(Combatant.Location, 12))
            {
                MeteorSwarmAttack();
                m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            // New Ability: Searing Overload (triggered if health falls below 30%)
            else if (DateTime.UtcNow >= m_NextSearingOverloadTime && this.Hits < (this.HitsMax * 0.3))
            {
                SearingOverload();
                m_NextSearingOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 45));
            }
        }

        // --- Unique Ability: Flame Nova Attack ---
        public void FlameNovaAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x208);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    targets.Add(m);
                }
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(50, 70);
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

            if (!(Combatant is Mobile target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Summoning hellfire meteors!*");
            PlaySound(0x160);

            int meteorCount = Utility.RandomMinMax(5, 8);
            for (int i = 0; i < meteorCount; i++)
            {
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

                Point3D startPoint = new Point3D(
                    impactPoint.X + Utility.RandomMinMax(-1, 1),
                    impactPoint.Y + Utility.RandomMinMax(-1, 1),
                    impactPoint.Z + 30);

                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map),
                                            new Entity(Serial.Zero, impactPoint, this.Map),
                                            0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

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
                            int damage = Utility.RandomMinMax(30, 45);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- New Unique Ability: Searing Overload ---
        // When the Hellfire Protector's health is below 30%, it unleashes a devastating burst damaging nearby foes (and itself slightly)
        public void SearingOverload()
        {
            if (Map == null)
                return;

            this.Say("*You will feel my wrath!*");
            PlaySound(0x20E);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 15, 50, UniqueHue, 0, 5052, 0);

            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 4);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    int damage = Utility.RandomMinMax(60, 80);
                    AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                }
            }
            eable.Free();

            // Self-damage (for balance)
            AOS.Damage(this, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
        }

        // --- Death Explosion: Leaving Fiery Debris ---
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

        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 150.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            if (Utility.RandomDouble() < 0.01)
                PackItem(new InfernosEmbraceCloak());

            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
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

            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextSearingOverloadTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
        }
    }
}
