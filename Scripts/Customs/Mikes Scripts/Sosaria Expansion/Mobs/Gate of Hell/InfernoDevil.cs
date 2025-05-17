using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For spell effects
using Server.Network; // For particle and sound effects
using System.Collections.Generic; // For lists used in area effects
using Server.Spells.Fourth; // For firefield effects

namespace Server.Mobiles
{
    [CorpseName("an infernal devil corpse")]
    public class InfernoDevil : BaseCreature
    {
        // Ability cooldown timers (to avoid ability spam)
        private DateTime m_NextAuraTime;
        private DateTime m_NextNovaTime;
        private DateTime m_NextMeteorTime;
        private DateTime m_NextLashTime;

        // Used for tracking movement-based effects
        private Point3D m_LastLocation;

        // A unique hue for the fire-touched monster
        private const int UniqueHue = 1199;

        [Constructable]
        public InfernoDevil() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "an Inferno Devil";
            Body = 792;               // Same as ChaosDaemon body
            BaseSoundID = 0x3E9;        // Same as ChaosDaemon sound
            Hue = UniqueHue;          // Unique fiery hue

            // --- Boosted Stats ---
            SetStr(500, 600);
            SetDex(250, 300);
            SetInt(400, 500);

            SetHits(1400, 1700);
            SetStam(250, 300);
            SetMana(400, 500);

            SetDamage(25, 35);

            // --- Damage Distribution: Mostly Fire with a splash of physical ---
            SetDamageType(ResistanceType.Physical, 15);
            SetDamageType(ResistanceType.Fire, 85);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 60, 70);
            SetResistance(ResistanceType.Fire, 90, 100);
            SetResistance(ResistanceType.Cold, 0, 10);   // Vulnerability to cold is minimal
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 105.1, 120.0);
            SetSkill(SkillName.Magery, 105.1, 120.0);
            SetSkill(SkillName.MagicResist, 115.2, 130.0);
            SetSkill(SkillName.Tactics, 105.1, 115.0);
            SetSkill(SkillName.Wrestling, 105.1, 115.0);
            SetSkill(SkillName.Anatomy, 90.0, 100.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 5;

            // --- Initialize Ability Cooldowns ---
            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextLashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));

            // --- Basic Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(10, 15)));
            PackItem(new MandrakeRoot(Utility.RandomMinMax(5, 8)));

            // Keep a light source attached (if needed)
            AddItem(new LightSource());

            m_LastLocation = this.Location;
        }

        // --- Unique Ability: Infernal Blaze Aura ---
        // When a mobile moves near the Inferno Devil, leave behind a burning field.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && m.InRange(this.Location, 2))
            {
                // Leave a fire field at the mobile's old location.
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(5 + Utility.Random(5));
                int damage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Play visual and sound effects (Flamestrike-style)
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                }
            }

            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // If the Inferno Devil has moved, spawn a fire field at its previous location.
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

            // Use special abilities based on cooldowns and range to the target
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(Combatant.Location, 8))
            {
                DiabolicFlameNova();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
            else if (DateTime.UtcNow >= m_NextMeteorTime && this.InRange(Combatant.Location, 12))
            {
                HellMeteorShower();
                m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextLashTime && this.InRange(Combatant.Location, 2))
            {
                HellfireLash();
                m_NextLashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
        }

        // --- Unique Ability: Diabolic Flame Nova ---
        // Unleashes a fiery nova damaging all nearby targets.
        public void DiabolicFlameNova()
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
                    targets.Add(m);
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

        // --- Unique Ability: Hell Meteor Shower ---
        // Rains a series of fiery meteors in a small area around the target.
        public void HellMeteorShower()
        {
            if (Combatant == null || Map == null)
                return;

            // Ensure Combatant is a Mobile before proceeding
            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Scorching rain of brimstone!*");
            PlaySound(0x160);

            int meteorCount = Utility.RandomMinMax(4, 7);
            for (int i = 0; i < meteorCount; i++)
            {
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-3, 3),
                    targetLocation.Y + Utility.RandomMinMax(-3, 3),
                    targetLocation.Z
                );

                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Spawn a meteor visual from a high starting point
                Point3D startPoint = new Point3D(
                    impactPoint.X + Utility.RandomMinMax(-1, 1),
                    impactPoint.Y + Utility.RandomMinMax(-1, 1),
                    impactPoint.Z + 30
                );

                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, this.Map), new Entity(Serial.Zero, impactPoint, this.Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay to allow meteor to “land” and apply damage
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (this.Map == null)
                        return;

                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 2023, 0);
                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(30, 40);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Unique Ability: Hellfire Lash ---
        // A brutal, close-range attack that lashes the target with searing energy.
        public void HellfireLash()
        {
            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target))
                return;

            this.Say("*Feel the lash of hellfire!*");
            PlaySound(0x208);
            
            // Visual effect on the target
            target.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
            DoHarmful(target);
            int damage = Utility.RandomMinMax(40, 60);
            AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
        }

        // --- Death Explosion ---
        // On death the Inferno Devil spawns a burst of lava tiles that deal damage.
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

                Effects.SendLocationParticles(EffectItem.Create(lavaLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 145.0; } }
        public override double DispelFocus { get { return 70.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(6, 10));

            if (Utility.RandomDouble() < 0.01) // 1% chance
            {
                PackItem(new InfernosEmbraceCloak());
            }
            if (Utility.RandomDouble() < 0.05) // 5% chance
            {
                PackItem(new DaemonBone(Utility.RandomMinMax(3, 5)));
            }
        }

        public InfernoDevil(Serial serial)
            : base(serial)
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

            m_NextAuraTime = DateTime.UtcNow;
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(6, 10));
            m_NextMeteorTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextLashTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
        }
    }
}
