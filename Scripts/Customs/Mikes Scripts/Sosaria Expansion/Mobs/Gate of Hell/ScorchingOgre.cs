using System;
using System.Collections.Generic;
using Server;
using Server.Factions;
using Server.Items;
using Server.Mobiles;
using Server.Network;
using Server.Spells;
using Server.Spells.Fourth; // For FireFieldSpell
using System.Collections; // For IPooledEnumerable

namespace Server.Mobiles
{
    [CorpseName("a charred ogre lord's corpse")]
    public class ScorchingOgre : BaseCreature
    {
        // Timers to control ability cooldowns
        private DateTime m_NextAuraTime;
        private DateTime m_NextSingeTime;
        private DateTime m_NextBurstTime;
        private Point3D m_LastLocation;

        // Unique fire-themed hue (adjust as desired)
        private const int UniqueHue = 1161;

        [Constructable]
        public ScorchingOgre()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Scorching Ogre";
            Body = 83;              // Same as OgreLord
            BaseSoundID = 427;      // Same as OgreLord
            Hue = UniqueHue;        // Unique fiery hue


            // --- Elevated Stats ---
            SetStr(900, 1100);
            SetDex(80, 100);
            SetInt(70, 90);

            SetHits(600, 750);
            SetStam(200, 250);
            SetMana(100, 150);

            SetDamage(25, 35);

            // Split damage between physical and fire for thematic effect
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 50);

            // --- Enhanced Resistances ---
            SetResistance(ResistanceType.Physical, 55, 70);
            SetResistance(ResistanceType.Fire, 80, 90); // Highly fire resistant
            SetResistance(ResistanceType.Cold, 20, 30);   // Moderately resistant, but still susceptible
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 50, 60);

            // --- Skills ---
            SetSkill(SkillName.MagicResist, 130.0, 145.0);
            SetSkill(SkillName.Tactics, 100.0, 115.0);
            SetSkill(SkillName.Wrestling, 100.0, 115.0);

            Fame = 20000;
            Karma = -20000;
            VirtualArmor = 80;
            ControlSlots = 5;  // Boss-level creature

            // Basic starting loot; you can add more here if desired
            PackItem(new Club());

            // Initialize ability cooldowns
            m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextSingeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));

            // Initialize last known location for trail effect
            m_LastLocation = this.Location;
        }

        public ScorchingOgre(Serial serial)
            : base(serial)
        {
        }

        // --- Burning Trail: Leave behind a fire field as you move.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                int itemID = 0x398C; // Fire field graphic
                TimeSpan duration = TimeSpan.FromSeconds(6 + Utility.Random(4));
                int damage = 3;      // Damage dealt by the trail

                // Spawn a fire field at the mobile's previous location
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);

                // Visual and sound effects
                m.FixedParticles(0x3709, 10, 30, 5052, EffectLayer.CenterFeet);
                m.PlaySound(0x208);

                // If Combatant is a Mobile, apply direct damage
                if (Combatant is Mobile target)
                {
                    if (CanBeHarmful(target, false) && SpellHelper.ValidIndirectTarget(this, target))
                    {
                        DoHarmful(target);
                        AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            // Leave a burning trail at our old location when we move
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;
                int itemID = 0x398C;
                TimeSpan duration = TimeSpan.FromSeconds(8 + Utility.Random(4));
                int damage = 3;
                var field = new Server.Spells.Fourth.FireFieldSpell.FireFieldItem(itemID, oldLocation, this, this.Map, duration, damage);
            }

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // --- Special Attacks ---

            // Burning Aura Attack: When an enemy is very close, deliver an AoE melee attack.
            if (DateTime.UtcNow >= m_NextAuraTime && this.InRange(Combatant.Location, 2))
            {
                BurningSmashAttack();
                m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            // Searing Slam Attack: Single-target heavy damage when in direct melee range.
            else if (DateTime.UtcNow >= m_NextSingeTime && this.InRange(Combatant.Location, 1))
            {
                SearingSlamAttack();
                m_NextSingeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            // Magma Burst Attack: Rains fire over an area when enemies are nearby.
            else if (DateTime.UtcNow >= m_NextBurstTime && this.InRange(Combatant.Location, 8))
            {
                MagmaBurstAttack();
                m_NextBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
        }

        // --- BurningSmashAttack: AoE melee attack with a fiery shockwave.
        public void BurningSmashAttack()
        {
            if (Map == null)
                return;

            // Play visual and sound effects
            PlaySound(0x208);
            FixedParticles(0x3709, 10, 30, 5052, EffectLayer.LeftFoot);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 3); // 3-tile radius area

            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            if (targets.Count > 0)
            {
                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5029, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(20, 30);
                    AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);
                    target.FixedParticles(0x3779, 10, 25, 5032, EffectLayer.Head);
                }
            }
        }

        // --- SearingSlamAttack: A brutal single-target attack that inflicts immediate and delayed burn damage.
        public void SearingSlamAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target))
                return;

            // Flavor text and sound effect
            this.Say("*The ogre's fist ignites with infernal flames!*");
            PlaySound(0x208);

            // Immediate damage
            DoHarmful(target);
            int damage = Utility.RandomMinMax(35, 50);
            AOS.Damage(target, this, damage, 0, 100, 0, 0, 0);

            // Delayed burning damage (simulating a burn-over-time effect)
            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target != null && target.Alive && CanBeHarmful(target, false))
                    AOS.Damage(target, this, Utility.RandomMinMax(10, 20), 0, 100, 0, 0, 0);
            });
        }

        // --- MagmaBurstAttack: Rains down fiery meteors in an area around the target.
        public void MagmaBurstAttack()
        {
            if (Combatant == null || Map == null)
                return;

            Mobile target = Combatant as Mobile;
            if (target == null || !CanBeHarmful(target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*The Scorching Ogre unleashes a burst of magma!*");
            PlaySound(0x160); // Meteor impact sound
            int burstCount = Utility.RandomMinMax(3, 6);

            for (int i = 0; i < burstCount; i++)
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

                // Launch a fireball-like meteor from above
                Point3D startPoint = new Point3D(impactPoint.X, impactPoint.Y, impactPoint.Z + 30);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, Map), new Entity(Serial.Zero, impactPoint, Map),
                    0x36D4, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Delay damage to coincide with visual impact
                Timer.DelayCall(TimeSpan.FromSeconds(0.5 + (i * 0.1)), () =>
                {
                    if (Map == null)
                        return;

                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 2023, 0);
                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(20, 30);
                            AOS.Damage(m, this, damage, 0, 100, 0, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- OnDeath: On death, create a fiery explosion and leave behind fire patches.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int FlamestrikeHazardTilees = 8;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < FlamestrikeHazardTilees; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < FlamestrikeHazardTilees - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D fireLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(fireLocation.X, fireLocation.Y, fireLocation.Z, 16, false, false))
                {
                    fireLocation.Z = Map.GetAverageZ(fireLocation.X, fireLocation.Y);
                    if (!Map.CanFit(fireLocation.X, fireLocation.Y, fireLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(fireLocation);

                // Assume FlamestrikeHazardTile is a custom item defined elsewhere that represents a lingering pool of fire
                FlamestrikeHazardTile patch = new FlamestrikeHazardTile();
                patch.Hue = UniqueHue;
                patch.MoveToWorld(fireLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(fireLocation, this.Map, EffectItem.DefaultDuration),
                    0x3709, 10, 20, UniqueHue, 0, 5016, 0);
            }

            // Central explosion effect
            Point3D deathPoint = this.Location;
            if (effectLocations.Count > 0)
                deathPoint = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathPoint, this.Map, 0x218);
            Effects.SendLocationParticles(EffectItem.Create(deathPoint, this.Map, EffectItem.DefaultDuration),
                0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Creature Overrides ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 4; } }
        public override double DispelDifficulty { get { return 120.0; } }
        public override double DispelFocus { get { return 50.0; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override int Meat { get { return 3; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 3);
            AddLoot(LootPack.MedScrolls, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 7));

        }

        public override Faction FactionAllegiance
        {
            get { return Minax.Instance; }
        }

        public override Ethics.Ethic EthicAllegiance
        {
            get { return Ethics.Ethic.Evil; }
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

            m_NextAuraTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(5, 8));
            m_NextSingeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextBurstTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 20));
        }
    }
}
