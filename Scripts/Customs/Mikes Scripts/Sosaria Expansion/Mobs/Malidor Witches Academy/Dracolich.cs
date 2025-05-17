using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;       // For spell effects potentially
using Server.Network;      // For visual/sound effects
using System.Collections.Generic;
using Server.Spells.Seventh;  // For chain lightning-like effects if desired

namespace Server.Mobiles
{
    [CorpseName("a dracolich corpse")]
    public class Dracolich : BaseCreature
    {
        // Cooldown timers for its unique abilities
        private DateTime m_NextBreathTime;
        private DateTime m_NextBoneShardsTime;
        private DateTime m_NextSoulSiphonTime;
        private DateTime m_NextCurseTime;
        private Point3D m_LastLocation;

        // Unique hue for this boss (choose a hue that stands out; 1175 is one example)
        private const int UniqueHue = 1175;

        [Constructable]
        public Dracolich() : base(AIType.AI_NecroMage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a dracolich";
            Body = 104;               // Same skeletal dragon body
            BaseSoundID = 0x488;        // Same base sound
            Hue = UniqueHue;            // Use our unique hue

            // --- Powerful Stats ---
            SetStr(1000, 1200);
            SetDex(200, 250);
            SetInt(600, 800);

            SetHits(750, 900);
            SetStam(300, 350);
            SetMana(500, 700);

            SetDamage(30, 40);
            // Damage is split into types to reflect its magical and physical nature
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Fire, 20);
            SetDamageType(ResistanceType.Energy, 30);

            // --- Enhanced Resistances ---
            SetResistance(ResistanceType.Physical, 70, 85);
            SetResistance(ResistanceType.Fire, 60, 80);
            SetResistance(ResistanceType.Cold, 60, 80);
            SetResistance(ResistanceType.Poison, 80, 90);
            SetResistance(ResistanceType.Energy, 70, 90);

            // --- Skills (magic-focused and necromantic) ---
            SetSkill(SkillName.EvalInt, 100.1, 120.0);
            SetSkill(SkillName.Magery, 110.1, 130.0);
            SetSkill(SkillName.MagicResist, 120.0, 140.0);
            SetSkill(SkillName.Tactics, 90.0, 100.0);
            SetSkill(SkillName.Wrestling, 90.0, 100.0);
            SetSkill(SkillName.Necromancy, 100.1, 120.0);
            SetSkill(SkillName.SpiritSpeak, 80.1, 100.0);

            Fame = 30000;
            Karma = -30000;
            VirtualArmor = 90;
            ControlSlots = 5;

            // --- Initialize Ability Cooldowns ---
            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoneShardsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSoulSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            // --- Loot: Advanced reagents and such ---
            PackItem(new BlackPearl(Utility.RandomMinMax(15, 20)));
            PackItem(new Nightshade(Utility.RandomMinMax(15, 20)));
            PackItem(new SulfurousAsh(Utility.RandomMinMax(15, 20)));

            m_LastLocation = this.Location;
        }

        // Standard overrides
        public override bool AutoDispel { get { return !Controlled; } }
        public override bool BleedImmune { get { return true; } }
        public override bool ReacquireOnMovement { get { return !Controlled; } }
        public override int Hides { get { return 25; } }
        public override int Meat { get { return 20; } }
        public override HideType HideType { get { return HideType.Barbed; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }
        public override Poison PoisonImmune { get { return Poison.Lethal; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }

        // --- OnMovement: Leave behind a damaging necrotic residue ---
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Check that m is a Mobile before accessing its specific properties
                if (m is Mobile target && SpellHelper.ValidIndirectTarget(this, target))
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(10, 15);
                    // Damage: mix of fire/energy to evoke necrotic magic
                    AOS.Damage(target, this, damage, 0, 0, 50, 50, 0);
                    target.FixedParticles(0x373A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.SendMessage(0x22, "A necrotic residue burns you!");
                }
            }

            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Choose among unique abilities based on cooldowns and range ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal || !Alive)
                return;

            if (DateTime.UtcNow >= m_NextBreathTime && this.InRange(Combatant.Location, 8))
            {
                NecroticBreathAttack();
                m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            else if (DateTime.UtcNow >= m_NextBoneShardsTime && this.InRange(Combatant.Location, 10))
            {
                BoneShardsBarrage();
                m_NextBoneShardsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
            else if (DateTime.UtcNow >= m_NextSoulSiphonTime && this.InRange(Combatant.Location, 6))
            {
                SoulSiphonAttack();
                m_NextSoulSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            }
            else if (DateTime.UtcNow >= m_NextCurseTime && this.InRange(Combatant.Location, 12))
            {
                CurseOfTheDamned();
                m_NextCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));
            }

            // Update last location for movement effects
            if (this.Location != m_LastLocation)
                m_LastLocation = this.Location;
        }

        // --- Ability 1: Necrotic Breath Attack ---
        public void NecroticBreathAttack()
        {
            if (Map == null)
                return;

            this.Say("*Withers your soul!*");
            PlaySound(0x0E6); // Dreadful breath sound

            // Visual effect: large conical burst (using particles)
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0);

            // Grab targets in a cone-like area (here all within 8 tiles)
            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 8);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(25, 35);
                // Deal combined damage (for example, 40% fire, 60% energy)
                AOS.Damage(target, this, damage, 0, 0, 40, 60, 0);
                // Also apply a poison effect to simulate necrosis
                target.ApplyPoison(target, Poison.Regular);
                target.FixedParticles(0x374A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                if (target is Mobile t)
                    t.SendMessage(0x22, "Your flesh is scorched by necrotic breath!");
            }
        }

        // --- Ability 2: Bone Shards Barrage ---
        public void BoneShardsBarrage()
        {
            if (Map == null)
                return;

            this.Say("*Feel the fury of ancient bones!*");
            PlaySound(0x20A); // Energy bolt sound

            List<Mobile> targets = new List<Mobile>();
            Mobile primaryTarget = Combatant as Mobile;
            if (primaryTarget == null || !CanBeHarmful(primaryTarget, false) || !SpellHelper.ValidIndirectTarget(this, primaryTarget))
                return;

            targets.Add(primaryTarget);
            int maxBounces = 4; // Bounce to additional targets
            int bounceRange = 5;

            // Chain-bounce logic: find up to maxBounces valid targets in succession
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

            // Damage each target in the chain with a visual bolt effect
            for (int i = 0; i < targets.Count; i++)
            {
                Mobile source = (i == 0) ? this : targets[i - 1];
                Mobile target = targets[i];

                Effects.SendMovingParticles(
                    new Entity(Serial.Zero, source.Location, source.Map),
                    new Entity(Serial.Zero, target.Location, target.Map),
                    0x3818, 7, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0x100);

                Timer.DelayCall(TimeSpan.FromSeconds(i * 0.15), () =>
                {
                    if (CanBeHarmful(target, false))
                    {
                        DoHarmful(target);
                        int damage = Utility.RandomMinMax(20, 30);
                        AOS.Damage(target, this, damage, 0, 0, 0, 0, 100); // Pure energy damage
                        target.FixedParticles(0x374A, 1, 15, 9502, UniqueHue, 0, EffectLayer.Waist);
                    }
                });
            }
        }

        // --- Ability 3: Soul Siphon Attack ---
        public void SoulSiphonAttack()
        {
            if (Combatant == null || Map == null)
                return;

            // Ensure Combatant is a Mobile before proceeding
            if (Combatant is Mobile target && CanBeHarmful(target, false))
            {
                this.Say("*I thirst for your soul!*");
                PlaySound(0x1F2); // Chilling, draining sound

                // Visual effect to indicate soul drain
                Effects.SendLocationParticles(EffectItem.Create(target.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 10, 15, UniqueHue, 0, 5039, 0);

                int damage = Utility.RandomMinMax(30, 45);
                DoHarmful(target);
                AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);
                // Heal self for half the damage dealt
                this.Hits += damage / 2;
                this.SendMessage("The dracolich absorbs part of your life force!");
            }
        }

        // --- Ability 4: Curse of the Damned ---
        // Applies a curse that reduces resistances and slows movement (simulated with a message and particle effects)
        public void CurseOfTheDamned()
        {
            if (Map == null)
                return;

            this.Say("*Behold the curse of the damned!*");
            PlaySound(0x1EE); // Ghostly sound effect

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 10);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                target.SendMessage(0x22, "You are cursed by the dracolich, your strength wanes!");
                target.FixedParticles(0x376A, 10, 30, 5032, UniqueHue, 0, EffectLayer.Waist);
                // (Optional) Implement timer-based debuffs to temporarily reduce target stats
            }
        }

        // --- OnDeath: Final Necrotic Explosion with Hazardous Tiles ---
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*My malediction... endures...*");
            Effects.PlaySound(this.Location, this.Map, 0x211);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 60, UniqueHue, 0, 5052, 0);

            // Spawn several hazardous tiles around the corpse (using ManaDrainTile as an example)
            int tilesToSpawn = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < tilesToSpawn; i++)
            {
                int xOffset = Utility.RandomMinMax(-4, 4);
                int yOffset = Utility.RandomMinMax(-4, 4);
                Point3D tileLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);

                if (!Map.CanFit(tileLocation.X, tileLocation.Y, tileLocation.Z, 16, false, false))
                {
                    tileLocation.Z = Map.GetAverageZ(tileLocation.X, tileLocation.Y);
                    if (!Map.CanFit(tileLocation.X, tileLocation.Y, tileLocation.Z, 16, false, false))
                        continue;
                }

                // You may choose from a variety of hazard tiles (e.g., FlamestrikeHazardTile, PoisonTile, etc.)
                ManaDrainTile drainTile = new ManaDrainTile();
                drainTile.Hue = UniqueHue;
                drainTile.MoveToWorld(tileLocation, this.Map);

                Effects.SendLocationParticles(EffectItem.Create(tileLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich, 3);
            AddLoot(LootPack.HighScrolls, 2);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(10, 15));

            // 3% chance for a unique dracolich artifact (replace MaxxiaScroll with your custom item, if desired)
            if (Utility.RandomDouble() < 0.03)
            {
                PackItem(new MaxxiaScroll());
            }
        }

        public Dracolich(Serial serial) : base(serial)
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

            m_NextBreathTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextBoneShardsTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSoulSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 30));
            m_NextCurseTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(25, 35));

            m_LastLocation = this.Location;
        }
    }
}
