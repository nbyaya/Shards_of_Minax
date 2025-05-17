using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;      // For potential spell effects
using Server.Network;     // For visual/sound effects

namespace Server.Mobiles
{
    [CorpseName("a mountain elite corpse")]
    public class MountainElite : BaseCreature
    {
        // Unique hue for the Mountain Elite (a deep, rugged stone tone)
        private const int UniqueHue = 1175;

        // Timers for unique abilities
        private DateTime m_NextSeismicWaveTime;
        private DateTime m_NextBoulderBarrageTime;
        private DateTime m_NextFortifyTime;
        
        // Used for leaving behind hazardous ground effects (rockslides)
        private Point3D m_LastLocation;
        private bool m_Fortified;

        [Constructable]
        public MountainElite() 
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.3)
        {
            Name = "a Mountain Elite";
            Body = 0x190;           // Using the same body value as Chaos Dragoon Elite
            BaseSoundID = 0x2CE;      // Using the base idle sound from Chaos Dragoon Elite
            Hue = UniqueHue;

            // --- Significantly Boosted Stats ---
            SetStr(500, 600);
            SetDex(200, 250);
            SetInt(250, 300);

            SetHits(2000, 2400);
            SetStam(300, 350);
            SetMana(300, 350);

            SetDamage(35, 45);      // Increased melee damage

            // 100% Physical damage
            SetDamageType(ResistanceType.Physical, 100);

            // --- Enhanced Resistances ---
            SetResistance(ResistanceType.Physical, 55, 65);
            SetResistance(ResistanceType.Fire, 40, 50);
            SetResistance(ResistanceType.Cold, 60, 70);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 45, 55);

            // --- Advanced Skill Levels ---
            SetSkill(SkillName.Tactics, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 120.2, 140.0);
            SetSkill(SkillName.Anatomy, 100.1, 120.0);
            SetSkill(SkillName.Swords, 110.1, 130.0);
            SetSkill(SkillName.Fencing, 100.1, 120.0);
            SetSkill(SkillName.Macing, 100.1, 120.0);

            Fame = 25000;
            Karma = -25000;

            VirtualArmor = 100;
            ControlSlots = 5;

            // --- Ability Timers Initialization ---
            m_NextSeismicWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextFortifyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
            m_LastLocation = this.Location;
        }

        // --- Hazardous Ground Effect ---
        // As the Mountain Elite moves, it occasionally leaves behind an EarthquakeTile,
        // simulating a sudden rockslide in its wake.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // 20% chance to drop a hazardous tile on movement
                if (Utility.RandomDouble() < 0.20 && this.Map != null && this.Map != Map.Internal)
                {
                    Point3D dropLocation = oldLocation;
                    if (Map.CanFit(dropLocation.X, dropLocation.Y, dropLocation.Z, 16, false, false))
                    {
                        EarthquakeTile tile = new EarthquakeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(dropLocation, this.Map);
                    }
                    else
                    {
                        int validZ = Map.GetAverageZ(dropLocation.X, dropLocation.Y);
                        if (Map.CanFit(dropLocation.X, dropLocation.Y, validZ, 16, false, false))
                        {
                            EarthquakeTile tile = new EarthquakeTile();
                            tile.Hue = UniqueHue;
                            tile.MoveToWorld(new Point3D(dropLocation.X, dropLocation.Y, validZ), this.Map);
                        }
                    }
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- Ability Handling in the Think Loop ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on cooldowns and range
            if (DateTime.UtcNow >= m_NextBoulderBarrageTime && this.InRange(Combatant.Location, 10))
            {
                BoulderBarrageAttack();
                m_NextBoulderBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
            else if (DateTime.UtcNow >= m_NextSeismicWaveTime && this.InRange(Combatant.Location, 8))
            {
                SeismicWaveAttack();
                m_NextSeismicWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            else if (DateTime.UtcNow >= m_NextFortifyTime)
            {
                FortifyDefense();
                m_NextFortifyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
            }
        }

        // --- Unique Ability: Seismic Wave Attack ---
        // Emits a shockwave dealing AoE physical damage to nearby targets.
        public void SeismicWaveAttack()
        {
            if (Map == null)
                return;

            this.Say("*The mountain trembles!*");
            PlaySound(0x2C8); // Sound effect for the attack
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.Waist); // Earth-shaking visual effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 7);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(50, 70);
                // Deal 100% physical damage
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                // Attempt a brief daze (simulate stun)
                if (target is Mobile targetMobile)
                {
                    targetMobile.SendMessage(0x22, "The shockwave leaves you dazed!");
                    // Optionally, call a method like targetMobile.Paralyze(TimeSpan.FromSeconds(2));
                }

                target.FixedParticles(0x3779, 10, 25, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // --- Unique Ability: Boulder Barrage Attack ---
        // Launches a chain of boulders that bounce among nearby foes.
        public void BoulderBarrageAttack()
        {
            if (Combatant == null || Map == null)
                return;

            this.Say("*Feel the crushing weight of stone!*");
            PlaySound(0x2D1); // Sound effect for the barrage

            List<Mobile> targets = new List<Mobile>();
            // Ensure Combatant is a Mobile before proceeding
            if (!(Combatant is Mobile primary) || !CanBeHarmful(primary, false) || !SpellHelper.ValidIndirectTarget(this, primary))
                return;

            targets.Add(primary);
            int maxBounces = 5;
            int range = 5;

            for (int i = 0; i < maxBounces; ++i)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = -1.0;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, range);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false)
                        && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (closestDist == -1.0 || dist < closestDist)
                        {
                            closestDist = dist;
                            nextTarget = m;
                        }
                    }
                }
                eable.Free();

                if (nextTarget != null)
                    targets.Add(nextTarget);
                else
                    break;
            }

            for (int i = 0; i < targets.Count; ++i)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Visual boulder effect (using a custom graphic ID to suggest a rocky projectile)
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x36D4, // Boulder graphic (choose an appropriate effect)
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Waist, 0x100);

                Mobile damageTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(damageTarget, false))
                    {
                        DoHarmful(damageTarget);
                        int damage = Utility.RandomMinMax(30, 45);
                        AOS.Damage(damageTarget, this, damage, 100, 0, 0, 0, 0);
                        damageTarget.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // --- Unique Ability: Fortify Defense ---
        // Temporarily increases the creature's VirtualArmor to simulate a fortified stony hide.
        public void FortifyDefense()
        {
            if (m_Fortified)
                return; // Already fortified

            this.Say("*I am unyielding as the mountain!*");
            PlaySound(0x2CC); // Sound effect for fortification

            int originalArmor = VirtualArmor;
            VirtualArmor += 50;  // Boost the defenses
            m_Fortified = true;

            // Shimmering rock aura visual effect
            FixedParticles(0x3709, 10, 30, 5052, UniqueHue, 0, EffectLayer.CenterFeet);

            // Revert the bonus after 10 seconds
            Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
            {
                VirtualArmor = originalArmor;
                m_Fortified = false;
                this.SendMessage("The mountain's protective power fades.");
            });
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(5, 10));

            // 3% chance to drop a unique mountain artifact (replace MountainHeart with an actual defined item)
            if (Utility.RandomDouble() < 0.03)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        // Standard creature properties
        public override bool AutoDispel { get { return true; } }
        public override bool BardImmune { get { return true; } }
        public override bool CanRummageCorpses { get { return true; } }
        public override bool AlwaysMurderer { get { return true; } }
        public override bool ShowFameTitle { get { return false; } }

        public override int GetIdleSound() { return 0x2CE; }
        public override int GetDeathSound() { return 0x2CC; }
        public override int GetHurtSound() { return 0x2D1; }
        public override int GetAttackSound() { return 0x2C8; }

        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override int TreasureMapLevel { get { return 6; } }
        public override double DispelDifficulty { get { return 160.0; } }
        public override double DispelFocus { get { return 80.0; } }

        public MountainElite(Serial serial)
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

            // Reinitialize timers on load
            m_NextSeismicWaveTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoulderBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextFortifyTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 35));
        }
    }
}
