using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For spell effects if needed
using Server.Network; // For playing particle effects and sounds

namespace Server.Mobiles
{
    [CorpseName("a stony golath corpse")]
    public class StonyGolath : BaseCreature
    {
        // Ability timers
        private DateTime m_NextBoulderBarrageTime;
        private DateTime m_NextEarthshatterTime;

        // Unique hue for this monster – gives it a distinct, stony tint
        private const int UniqueHue = 1175;

        [Constructable]
        public StonyGolath() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Stony Golath";
            Body = 248;           // Based on Gaman
            BaseSoundID = 0x4F8;    // Use Gaman's sound base
            Hue = UniqueHue;        // Unique stony hue

            // --- Advanced Stats for a powerful boss ---
            SetStr(550, 650);
            SetDex(350, 400);
            SetInt(300, 350);

            SetHits(2000, 2200);
            SetStam(400, 500);
            SetMana(500, 600);

            SetDamage(20, 30);

            // Damage is entirely physical
            SetDamageType(ResistanceType.Physical, 100);

            // Resistances (stony creatures are tough and cold-adapted)
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 40, 60);
            SetResistance(ResistanceType.Cold, 80, 90);
            SetResistance(ResistanceType.Poison, 50, 70);
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills: Mix of melee and magic resist
            SetSkill(SkillName.Tactics, 100.0, 110.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.MagicResist, 120.0, 130.0);

            Fame = 30000;
            Karma = -30000;

            VirtualArmor = 90;
            ControlSlots = 5;

            // Initialize ability timers
            m_NextBoulderBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextEarthshatterTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }

        public override void OnThink()
        {
            base.OnThink();

            // If there is no valid combatant or map, do nothing
            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            // Prioritize abilities based on proximity:
            // - Boulder Barrage if Combatant is within 10 tiles
            // - Otherwise, Earthshatter Slam when near (within 3 tiles)
            if (DateTime.UtcNow >= m_NextBoulderBarrageTime && InRange(Combatant.Location, 10))
            {
                BoulderBarrageAttack();
                m_NextBoulderBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextEarthshatterTime && InRange(Combatant.Location, 3))
            {
                EarthshatterSlamAttack();
                m_NextEarthshatterTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }
        }

        // --- Unique Ability: Boulder Barrage Attack ---
        // Hurls a series of stone projectiles that bounce from one foe to another.
        public void BoulderBarrageAttack()
        {
            if (Map == null) return;

            // Confirm the combatant is a Mobile target
            if (!(Combatant is Mobile initialTarget) || !CanBeHarmful(initialTarget, false))
                return;

            this.Say("*Stony Golath hurls crushing boulders!*");
            PlaySound(0x1F2); // Use an appropriate rock-throwing sound

            List<Mobile> targets = new List<Mobile>();
            targets.Add(initialTarget);

            int maxBounces = 4;    // Maximum extra targets
            int bounceRange = 5;   // Range within which the boulder can bounce

            // Chain to additional targets
            for (int i = 0; i < maxBounces; i++)
            {
                Mobile lastTarget = targets[targets.Count - 1];
                Mobile nextTarget = null;
                double closestDist = double.MaxValue;

                IPooledEnumerable eable = Map.GetMobilesInRange(lastTarget.Location, bounceRange);
                foreach (Mobile m in eable)
                {
                    if (m != this && m != lastTarget && !targets.Contains(m) && CanBeHarmful(m, false)
                        && SpellHelper.ValidIndirectTarget(lastTarget, m) && lastTarget.InLOS(m))
                    {
                        double dist = lastTarget.GetDistanceToSqrt(m);
                        if (dist < closestDist)
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

            // Process the chain attack
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                // Create a visual particle effect for the stone projectile:
                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x36D4, // (Assumed) stone projectile graphic
                    7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                // Apply damage with a slight delay for visual continuity:
                Mobile capturedTarget = target;
                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.2), () =>
                {
                    if (capturedTarget != null && CanBeHarmful(capturedTarget, false))
                    {
                        DoHarmful(capturedTarget);
                        int damage = Utility.RandomMinMax(30, 50);
                        // Physical damage only (100% physical)
                        AOS.Damage(capturedTarget, this, damage, 100, 0, 0, 0, 0);
                        capturedTarget.FixedParticles(0x36D4, 10, 20, 0x13F, UniqueHue, 0, EffectLayer.Head);
                    }
                });
            }
        }

        // --- Unique Ability: Earthshatter Slam Attack ---
        // Slams the ground, unleashing a shockwave that deals heavy damage in an AoE 
        // and spawns EarthquakeTile hazards in the surrounding area.
        public void EarthshatterSlamAttack()
        {
            if (Map == null) return;

            this.Say("*Stony Golath slams the earth with unyielding force!*");
            PlaySound(0x22B); // Rumble sound effect

            // Create a shockwave particle effect at the monster’s current location:
            Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration), 
                                          0x36D4, 15, 20, UniqueHue, 0, 5030, 0);

            // Get all mobiles within a 4-tile radius
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(Location, 4);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            // Apply damage to each target
            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(40, 60);
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0); // 100% physical damage
                target.FixedParticles(0x36D4, 10, 20, 0x13F, UniqueHue, 0, EffectLayer.Waist);
            }

            // Spawn a few EarthquakeTile hazards near Stony Golath to simulate fissures
            int hazards = Utility.RandomMinMax(2, 4);
            for (int i = 0; i < hazards; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                Point3D hazardLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                {
                    hazardLocation.Z = Map.GetAverageZ(hazardLocation.X, hazardLocation.Y);
                    if (!Map.CanFit(hazardLocation.X, hazardLocation.Y, hazardLocation.Z, 16, false, false))
                        continue;
                }

                EarthquakeTile eqTile = new EarthquakeTile();
                eqTile.Hue = UniqueHue;
                eqTile.MoveToWorld(hazardLocation, Map);
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, Map, EffectItem.DefaultDuration),
                                              0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }
        }

        // --- Passive Ability: Stony Aura ---
        // Damages foes who move too close, as if they are being crushed by the weight of stone.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                if (m is Mobile targetMobile && SpellHelper.ValidIndirectTarget(this, targetMobile))
                {
                    DoHarmful(targetMobile);
                    int damage = Utility.RandomMinMax(10, 15);
                    AOS.Damage(targetMobile, this, damage, 100, 0, 0, 0, 0);
                    targetMobile.SendMessage(0x22, "The crushing presence of stone weighs you down!");
                    targetMobile.FixedParticles(0x36A, 10, 15, 0x13F, UniqueHue, 0, EffectLayer.Head);
                    PlaySound(0x4F7); // Feedback sound
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- Death Effect: Avalanche of Stone ---
        // When Stony Golath dies, trigger a final effect that causes an avalanche of stone hazards to erupt.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The mountain weeps as Stony Golath crumbles to dust!*");
                PlaySound(0x1FB); // Crumbling/explosion sound effect

                Effects.SendLocationParticles(EffectItem.Create(Location, Map, EffectItem.DefaultDuration),
                                              0x36D4, 15, 20, UniqueHue, 0, 5030, 0);

                // Spawn several EarthquakeTile hazards near the corpse
                int hazardCount = Utility.RandomMinMax(3, 5);
                for (int i = 0; i < hazardCount; i++)
                {
                    int xOffset = Utility.RandomMinMax(-4, 4);
                    int yOffset = Utility.RandomMinMax(-4, 4);
                    Point3D hazardLoc = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                    if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                    {
                        hazardLoc.Z = Map.GetAverageZ(hazardLoc.X, hazardLoc.Y);
                        if (!Map.CanFit(hazardLoc.X, hazardLoc.Y, hazardLoc.Z, 16, false, false))
                            continue;
                    }

                    EarthquakeTile eqTile = new EarthquakeTile();
                    eqTile.Hue = UniqueHue;
                    eqTile.MoveToWorld(hazardLoc, Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLoc, Map, EffectItem.DefaultDuration),
                                                  0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }
            base.OnDeath(c);
        }

        // --- Standard Sound Overrides (using Gaman's base sounds) ---
        public override int GetAngerSound() { return 0x4F8; }
        public override int GetIdleSound() { return 0x4F7; }
        public override int GetAttackSound() { return 0x4F6; }
        public override int GetHurtSound() { return 0x4F9; }
        public override int GetDeathSound() { return 0x4F5; }

        // --- Meat and Hides (matching Gaman) ---
        public override int Meat { get { return 10; } }
        public override int Hides { get { return 15; } }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));
            AddLoot(LootPack.Gems, Utility.RandomMinMax(8, 12));

            // Rare chance for a unique stony relic drop
            if (Utility.RandomDouble() < 0.02)
            {
                // Replace 'StonyRelic' with your actual unique item
                PackItem(new MaxxiaScroll());
            }
        }

        // --- Serialization ---
        public StonyGolath(Serial serial) : base(serial)
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
            m_NextBoulderBarrageTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            m_NextEarthshatterTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }
    }
}
