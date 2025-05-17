using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells; // For potential spell effects
using Server.Network; // For visual/effect methods
using System.Collections.Generic;
using Server.Spells.Fourth; // For any tile/spell effects if needed

namespace Server.Mobiles
{
    [CorpseName("a shattered magical warhorse corpse")]
    public class MalidorWarhorse : BaseMount
    {
        // Timers for special abilities to prevent spamming
        private DateTime m_NextNovaTime;
        private DateTime m_NextChargeTime;
        private Point3D m_LastLocation;
        
        // Unique hue for magic-themed effects (choose an appropriate hue)
        private const int UniqueHue = 1375;

        [Constructable]
        public MalidorWarhorse() : this("the Malidor Warhorse")
        {
        }

        [Constructable]
        public MalidorWarhorse(string name)
            : base(name, 0x74, 0x3EA7, AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            BaseSoundID = 0xA8;
            BodyValue = 116;
            Hue = UniqueHue;

            // --- Advanced Stat Profile ---
            SetStr(600, 700);
            SetDex(100, 130);
            SetInt(200, 260);

            SetHits(800, 900);
            SetStam(250, 300);
            SetMana(300, 350);

            SetDamage(25, 32);

            // Damage is split: part physical, mostly arcane (energy)
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Energy, 70);

            // --- Resistances ---
            SetResistance(ResistanceType.Physical, 70, 80);
            SetResistance(ResistanceType.Fire, 30, 50);
            SetResistance(ResistanceType.Cold, 30, 50);
            SetResistance(ResistanceType.Poison, 60, 70);
            SetResistance(ResistanceType.Energy, 80, 90);

            // --- Enhanced Skills ---
            SetSkill(SkillName.EvalInt, 100.1, 115.0);
            SetSkill(SkillName.Magery, 100.1, 120.0);
            SetSkill(SkillName.MagicResist, 110.2, 125.0);
            SetSkill(SkillName.Tactics, 100.1, 110.0);
            SetSkill(SkillName.Wrestling, 100.1, 110.0);

            Fame = 20000;
            Karma = -20000;

            VirtualArmor = 80;
            ControlSlots = 4;

            // --- Ability Cooldowns ---
            m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_LastLocation = this.Location;

            // --- Loot ---
            PackItem(new SulfurousAsh(Utility.RandomMinMax(5, 10)));
            PackItem(new BlackPearl(Utility.RandomMinMax(3, 6)));
        }

        // --- Magic Themed Aura on Movement ---
        // When any mobile moves nearby, drain some mana with an arcane effect.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive)
            {
                // Check that the mobile is valid before accessing Mobile-specific properties
                if (m is Mobile target)
                {
                    if (target.Mana > 0)
                    {
                        int drainAmount = Utility.RandomMinMax(5, 10);
                        target.Mana -= drainAmount;
                        // Arcane particles effect
                        target.FixedParticles(0x375A, 10, 20, 5036, UniqueHue, 0, EffectLayer.Waist);
                        target.SendMessage("Arcane forces drain a bit of your mana!");
                    }
                }
            }
            base.OnMovement(m, oldLocation);
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || Map == Map.Internal)
                return;

            // --- Arcane Residue on Movement ---
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal)
            {
                Point3D oldLocation = m_LastLocation;
                m_LastLocation = this.Location;

                // Spawn an arcane residue tile (using ManaDrainTile for a magic effect)
                if (Map.CanFit(oldLocation.X, oldLocation.Y, oldLocation.Z, 16, false, false))
                {
                    var residue = new ManaDrainTile();
                    residue.Hue = UniqueHue;
                    residue.MoveToWorld(oldLocation, Map);
                    Effects.SendLocationParticles(EffectItem.Create(oldLocation, Map, EffectItem.DefaultDuration), 0x375A, 10, 20, UniqueHue, 0, 5036, 0);
                }
            }

            // --- Check for Arcane Nova Ability ---
            if (DateTime.UtcNow >= m_NextNovaTime && this.InRange(Combatant.Location, 6))
            {
                ArcaneNovaAttack();
                m_NextNovaTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }
            // --- Check for Mystic Charge Ability ---
            else if (DateTime.UtcNow >= m_NextChargeTime && this.InRange(Combatant.Location, 8))
            {
                MysticChargeAttack();
                m_NextChargeTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(18, 25));
            }
        }

        // --- Unique Ability: Arcane Nova ---
        // Emits a burst of arcane energy that deals 100% energy damage to all nearby foes.
        public void ArcaneNovaAttack()
        {
            if (Map == null)
                return;

            PlaySound(0x5D); // Magical sound effect
            FixedParticles(0x376A, 10, 30, 5032, UniqueHue, 0, EffectLayer.Waist);

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
                Effects.SendLocationParticles(EffectItem.Create(this.Location, Map, EffectItem.DefaultDuration), 0x376A, 10, 40, UniqueHue, 0, 5032, 0);
                foreach (Mobile target in targets)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(30, 45);
                    // Deal pure energy (arcane) damage
                    AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);
                    target.FixedParticles(0x375A, 10, 25, 5036, UniqueHue, 0, EffectLayer.Head);
                    if (target is Mobile t)
                        t.SendMessage("The burst of arcane power shocks you!");
                }
            }
        }

        // --- Unique Ability: Mystic Charge ---
        // The warhorse charges, releasing arcane bolts that home in on nearby foes.
        public void MysticChargeAttack()
        {
            if (Combatant == null || Map == null)
                return;

            // Ensure Combatant is a Mobile before using Mobile-specific properties
            if (!(Combatant is Mobile target) || !CanBeHarmful(target))
                return;

            Point3D targetLocation = target.Location;
            this.Say("*Unleashing mystic charge!*");
            PlaySound(0x20F); // Charge/magic sound effect

            int boltCount = Utility.RandomMinMax(3, 5);
            for (int i = 0; i < boltCount; i++)
            {
                Point3D impactPoint = new Point3D(
                    targetLocation.X + Utility.RandomMinMax(-2, 2),
                    targetLocation.Y + Utility.RandomMinMax(-2, 2),
                    targetLocation.Z);

                if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                {
                    impactPoint.Z = Map.GetAverageZ(impactPoint.X, impactPoint.Y);
                    if (!Map.CanFit(impactPoint.X, impactPoint.Y, impactPoint.Z, 16, false, false))
                        continue;
                }

                // Create a visual arcane projectile from above toward the impact point
                Point3D startPoint = new Point3D(impactPoint.X + Utility.RandomMinMax(-1, 1), impactPoint.Y + Utility.RandomMinMax(-1, 1), impactPoint.Z + 15);
                Effects.SendMovingParticles(new Entity(Serial.Zero, startPoint, Map),
                    new Entity(Serial.Zero, impactPoint, Map),
                    0x3728, 5, 0, false, false, UniqueHue, 0, 9502, 1, 0, EffectLayer.Head, 0);

                Timer.DelayCall(TimeSpan.FromSeconds(0.3 + (i * 0.1)), () =>
                {
                    if (Map == null)
                        return;
                    Effects.SendLocationParticles(EffectItem.Create(impactPoint, Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5032, 0);
                    IPooledEnumerable affected = Map.GetMobilesInRange(impactPoint, 1);
                    foreach (Mobile m in affected)
                    {
                        if (CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                        {
                            DoHarmful(m);
                            int damage = Utility.RandomMinMax(20, 30);
                            // Apply pure energy damage
                            AOS.Damage(m, this, damage, 0, 0, 100, 0, 0);
                        }
                    }
                    affected.Free();
                });
            }
        }

        // --- Death Explosion ---
        // Upon death, the warhorse releases a burst of mana-draining energy by spawing ManaDrainTile tiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            int tileCount = 10;
            List<Point3D> effectLocations = new List<Point3D>();

            for (int i = 0; i < tileCount; i++)
            {
                int xOffset = Utility.RandomMinMax(-3, 3);
                int yOffset = Utility.RandomMinMax(-3, 3);
                if (xOffset == 0 && yOffset == 0 && i < tileCount - 1)
                    xOffset = Utility.RandomBool() ? 1 : -1;

                Point3D tileLocation = new Point3D(this.X + xOffset, this.Y + yOffset, this.Z);
                if (!Map.CanFit(tileLocation.X, tileLocation.Y, tileLocation.Z, 16, false, false))
                {
                    tileLocation.Z = Map.GetAverageZ(tileLocation.X, tileLocation.Y);
                    if (!Map.CanFit(tileLocation.X, tileLocation.Y, tileLocation.Z, 16, false, false))
                        continue;
                }

                effectLocations.Add(tileLocation);
                ManaDrainTile tile = new ManaDrainTile();
                tile.Hue = UniqueHue;
                tile.MoveToWorld(tileLocation, Map);
                Effects.SendLocationParticles(EffectItem.Create(tileLocation, Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5032, 0);
            }

            Point3D deathLocation = this.Location;
            if (effectLocations.Count > 0)
                deathLocation = effectLocations[Utility.Random(effectLocations.Count)];

            Effects.PlaySound(deathLocation, Map, 0x3AF); // A magical explosion sound effect
            Effects.SendLocationParticles(EffectItem.Create(deathLocation, Map, EffectItem.DefaultDuration), 0x376A, 10, 40, UniqueHue, 0, 5052, 0);

            base.OnDeath(c);
        }

        // --- Standard Properties & Loot ---
        public override bool BleedImmune { get { return true; } }
        public override int TreasureMapLevel { get { return 5; } }
        public override double DispelDifficulty { get { return 140.0; } }
        public override double DispelFocus { get { return 65.0; } }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.FilthyRich, 2);
            AddLoot(LootPack.Average);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(4, 7));

            // Chance for a unique magical drop; adjust items as needed
            if (Utility.RandomDouble() < 0.01)
                PackItem(new MaxxiaScroll());
            if (Utility.RandomDouble() < 0.05)
                PackItem(new MaxxiaScroll(Utility.RandomMinMax(3, 6)));
        }

        public MalidorWarhorse(Serial serial)
            : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)1); // version
            writer.Write(m_NextNovaTime);
            writer.Write(m_NextChargeTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextNovaTime = reader.ReadDateTime();
            m_NextChargeTime = reader.ReadDateTime();
        }
    }
}
