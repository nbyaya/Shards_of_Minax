using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;            // For potential spell effects
using Server.Network;           // For visual and sound effects
using System.Collections.Generic; // For list-based AoE targeting
using Server.Spells.Fourth;     // For FireField spells

namespace Server.Mobiles
{
    [CorpseName("the scorched corpse of a blazing titan")]
    public class BlazingTitan : BaseCreature
    {
        // Timers for special abilities to prevent spamming
        private DateTime m_NextFlameCharge;
        private DateTime m_NextFireNova;
        private DateTime m_NextFireRain;
        private Point3D m_LastLocation;

        // Unique Hue for this fire-themed titan
        private const int UniqueHue = 1175;

        [Constructable]
        public BlazingTitan() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "a Blazing Titan";
            Body = 76;            // Same as the base Titan
            BaseSoundID = 609;    // Using Titan's sound
            Hue = UniqueHue;

            // --- Boosted Stats ---
            SetStr(700, 800);
            SetDex(150, 200);
            SetInt(300, 350);

            SetHits(1600, 2000);
            SetStam(250, 300);
            SetMana(400, 500);

            SetDamage(25, 30);

            // Damage types: a mix of physical and fire (leaning toward fire damage)
            SetDamageType(ResistanceType.Physical, 40);
            SetDamageType(ResistanceType.Fire, 60);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 20, 30);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 110.0, 120.0);
            SetSkill(SkillName.Magery, 110.0, 125.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);
            SetSkill(SkillName.Tactics, 110.0, 120.0);
            SetSkill(SkillName.Wrestling, 110.0, 120.0);
            SetSkill(SkillName.Anatomy, 100.0, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 90;
            ControlSlots = 5;
			Tamable = true;

            // Initialize ability timers
            m_NextFlameCharge = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextFireNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFireRain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_LastLocation = this.Location;

            // Example loot items (adjust as desired)
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 8)));

            // Keep a light source on the titan
            AddItem(new LightSource());
        }

        // --- Override OnMovement to leave behind a fiery trail ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                // Create a small fire field at the old location
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(3));
                int damage = 4;

                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Play fire effects on the mobile that is moving nearby
                m.FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Override OnThink to handle ability timers ---
        public override void OnThink()
        {
            base.OnThink();

            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(6 + Utility.Random(4));
                int damage = 4;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            // Ensure Combatant is a Mobile before proceeding
            if (!(Combatant is Mobile target) || Map == null || Map == Map.Internal)
                return;

            if (DateTime.UtcNow >= m_NextFireNova && InRange(target.Location, 8))
            {
                FireNovaAttack();
                m_NextFireNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextFireRain && InRange(target.Location, 12))
            {
                FireRainAttack();
                m_NextFireRain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 30));
            }

            // Additional ability: Flame Charge when target is very close
            if (DateTime.UtcNow >= m_NextFlameCharge && InRange(target.Location, 3))
            {
                FlameChargeAttack();
                m_NextFlameCharge = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // --- Special Ability: Fire Nova Attack ---
        public void FireNovaAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x208); // Fiery explosion sound
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 60, UniqueHue, 0, 5029, 0);

                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(50, 70);
                    // 100% fire damage
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
                }
            }
        }

        // --- Special Ability: Fire Rain Attack (Meteor-Like) ---
        public void FireRainAttack()
        {
            if (!(Combatant is Mobile target) || Map == null)
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Burning skies!*");
            PlaySound(0x160); // Meteor sound

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

                // Simulate meteoric descent with moving particles (using a Flamestrike graphic as a proxy)
                Point3D startPoint = new Point3D(
                    impactPoint.X + Utility.RandomMinMax(-1, 1),
                    impactPoint.Y + Utility.RandomMinMax(-1, 1),
                    impactPoint.Z + 30);

                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, Map),
                    new Entity(Serial.Zero, impactPoint, Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (Map == null)
                        return;

                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, Map, EffectItem.DefaultDuration),
                        0x3709, 10, 30, UniqueHue, 0, 2023, 0);

                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int dmg = Utility.RandomMinMax(30, 40);
                            AOS.Damage(m, this, dmg, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Special Ability: Flame Charge Attack ---
        public void FlameChargeAttack()
        {
            if (!(Combatant is Mobile target) || Map == null)
                return;

            this.Say("*Flame charge!*");
            PlaySound(0x225); // Distinct sound for a charging attack

            // Send moving particles along the charge path from the titan to the target
            Effects.SendMovingParticles(new Entity(Serial.Zero, this.Location, Map),
                new Entity(Serial.Zero, target.Location, Map),
                0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(0.3), () =>
            {
                if (target == null || !target.Alive || Map == null)
                    return;
                DoHarmful(target);
                int dmg = Utility.RandomMinMax(35, 50);
                AOS.Damage(target, this, dmg, 0, 100, 0, 0, 0);
                target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
            });
        }

        // --- Death Explosion ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int lavaTilesToDrop = 12; // Number of lava tiles to drop on death
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
                droppedLava.MoveToWorld(lavaLocation, Map);

                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Use one of the spawned positions as the epicenter of the final explosion
            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Overrides ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }

        // Increased difficulty for dispelling the titan
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 65.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 8));

            // Rare drop chances
            if (Utility.RandomDouble() < 0.01)
                PackItem(new TitanforgedStoneChest());
            if (Utility.RandomDouble() < 0.02)
                PackItem(new InfernosEmbraceCloak());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
        }

        public BlazingTitan(Serial serial) : base(serial)
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

            // Reset timers on load
            m_NextFlameCharge = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextFireNova = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextFireRain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
        }
    }
}
