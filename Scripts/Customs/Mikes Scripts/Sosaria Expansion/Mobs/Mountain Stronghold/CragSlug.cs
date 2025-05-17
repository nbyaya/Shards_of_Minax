using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;      // For potential spell effects
using Server.Network;     // For particle effects

namespace Server.Mobiles
{
    [CorpseName("a shattered crag slug corpse")]
    public class CragSlug : BaseCreature
    {
        // Ability timers
        private DateTime m_NextSpitTime;
        private DateTime m_NextEruptionTime;
        private DateTime m_NextCorrosionTime;

        // Unique hue for our advanced Crag Slug
        private const int UniqueHue = 1170;

        [Constructable]
        public CragSlug() : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Crag Slug";
            Body = 51;                 // Same body as AcidSlug
            BaseSoundID = 1499;        // Same base sounds as AcidSlug
            Hue = UniqueHue;

            // Significantly enhanced stats
            SetStr(400, 500);
            SetDex(100, 120);
            SetInt(50, 70);

            SetHits(800, 1000);

            SetDamage(30, 40);
            // Damage split: 30% Physical and 70% Poison
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Poison, 70);

            // Improved resistances
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Basic combat skills – no fancy magic here, just brute and resilience
            SetSkill(SkillName.MagicResist, 50.0, 70.0);
            SetSkill(SkillName.Tactics, 60.0, 80.0);
            SetSkill(SkillName.Wrestling, 60.0, 90.0);

            Fame = 15000;
            Karma = -15000;
            VirtualArmor = 40;
            ControlSlots = 3; // Boss-level creature but uses fewer control slots than a SpellElemental

            // Initialize ability timers with randomized cooldowns
            m_NextSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextCorrosionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));

            // Loot – based on AcidSlug but can be expanded
            PackItem(new AcidSac());
            PackItem(new CongealedSlugAcid());
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich);
        }

        // Reuse acid slug sounds
        public override int GetIdleSound() { return 1499; }
        public override int GetAngerSound() { return 1496; }
        public override int GetHurtSound() { return 1498; }
        public override int GetDeathSound() { return 1497; }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Check abilities based on cooldowns and distance to Combatant.

            // Toxic Spit Attack (ranged, up to 10 tiles)
            if (DateTime.UtcNow >= m_NextSpitTime && this.InRange(Combatant.Location, 10))
            {
                ToxicSpitAttack();
                m_NextSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            // Seismic Eruption Attack (area effect within 12 tiles)
            else if (DateTime.UtcNow >= m_NextEruptionTime && this.InRange(Combatant.Location, 12))
            {
                SeismicEruptionAttack();
                m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            // Acidic Corrosion Attack (close combat, within 3 tiles)
            else if (DateTime.UtcNow >= m_NextCorrosionTime && this.InRange(Combatant.Location, 3))
            {
                AcidicCorrosionAttack();
                m_NextCorrosionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
        }

        // --- Toxic Spit Attack ---
        // Fires a corrosive projectile that damages and leaves a hazardous poison tile on impact.
        public void ToxicSpitAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Point3D targetLocation;
            if (Combatant is Mobile targetMobile)
                targetLocation = targetMobile.Location;
            else
                targetLocation = Combatant.Location;

            // Visual and sound effects for spitting acid
            this.FixedParticles(0x3709, 10, 30, 5032, UniqueHue, 0, EffectLayer.Head);
            PlaySound(0x20A);

            // Create a projectile effect moving from CragSlug to target location
            Effects.SendMovingParticles(
                new Entity(Serial.Zero, this.Location, this.Map),
                new Entity(Serial.Zero, targetLocation, this.Map),
                0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

            // Delay the damage so the projectile is visible
            Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
            {
                if (Combatant == null || Map == null)
                    return;

                if (Combatant is Mobile target)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(40, 60);
                    // Apply damage: 30% physical, 70% poison
                    AOS.Damage(target, this, damage, 30, 0, 0, 70, 0);
                    target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.SendMessage(0x22, "The acidic spit burns you!");

                    // Leave behind a hazardous PoisonTile at the target's location
                    if (Map.CanFit(target.Location.X, target.Location.Y, target.Location.Z, 16, false, false))
                    {
                        PoisonTile pt = new PoisonTile();
                        pt.Hue = UniqueHue;
                        pt.MoveToWorld(target.Location, Map);
                    }
                }
            });
        }

        // --- Seismic Eruption Attack ---
        // Unleashes a shockwave that damages all nearby foes and spawns hazardous EarthquakeTiles beneath them.
        public void SeismicEruptionAttack()
        {
            if (Map == null)
                return;

            this.Say("*The ground trembles beneath the Crag Slug's weight!*");
            PlaySound(0x211);

            // Visual shockwave effect centered on the Crag Slug
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration),
                                          0x3709, 10, 40, UniqueHue, 0, 5052, 0);

            // Get all mobiles within a radius of 6 tiles
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 6);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(30, 50);
                // Deal damage equally split as 50% Physical and 50% Poison
                AOS.Damage(target, this, damage, 50, 0, 0, 50, 0);
                target.FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                target.SendMessage(0x22, "The shockwave smashes into you!");

                // Spawn a hazardous EarthquakeTile at the target's location
                if (Map.CanFit(target.Location.X, target.Location.Y, target.Location.Z, 16, false, false))
                {
                    EarthquakeTile eqTile = new EarthquakeTile();
                    eqTile.Hue = UniqueHue;
                    eqTile.MoveToWorld(target.Location, Map);
                }
            }
        }

        // --- Acidic Corrosion Attack ---
        // In melee range the Crag Slug bursts forth a corrosive cloud that damages and “corrodes” nearby foes.
        public void AcidicCorrosionAttack()
        {
            if (Map == null)
                return;

            this.Say("*Corrosive acid erupts from the Crag Slug!*");
            PlaySound(0x20E);

            // Targets within 2 tiles receive the effect.
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 2);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(20, 30);
                // Deal 100% Poison damage
                AOS.Damage(target, this, damage, 0, 0, 0, 100, 0);
                target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                target.SendMessage(0x22, "Your defenses corrode under the acid!");

                // If the target is a Mobile, apply a poison effect (using ApplyPoison safely)
                if (target is Mobile targetMobile)
                {
                    // Example: apply a lesser poison effect
                    targetMobile.ApplyPoison(this, Poison.Lesser);
                }
            }
        }

        // Override CheckMovement similar to the AcidSlug implementation.
        public override bool CheckMovement(Direction d, out int newZ)
        {
            if (!base.CheckMovement(d, out newZ))
                return false;

            if (Region.IsPartOf("Underworld") && newZ > Location.Z)
                return false;

            return true;
        }

        public CragSlug(Serial serial) : base(serial)
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

            // Reinitialize ability timers on load
            m_NextSpitTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextEruptionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextCorrosionTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
