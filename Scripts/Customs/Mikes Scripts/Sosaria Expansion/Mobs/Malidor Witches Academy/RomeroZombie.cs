using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;          // For potential spell effects
using Server.Network;         // For visual/sound effects
using System.Collections.Generic;  // For list-based AoE targeting

namespace Server.Mobiles
{
    [CorpseName("a corrupted undead corpse")]
    public class RomeroZombie : BaseCreature
    {
        // Unique ability cooldown timers and tracking variables.
        private DateTime m_NextSoulSiphonTime;
        private DateTime m_NextSummonTime;
        private Point3D m_LastLocation;

        // This unique hue gives the Romero Zombie a distinctive magical appearance.
        private const int UniqueHue = 1345;

        [Constructable]
        public RomeroZombie() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.1, 0.2)
        {
            Name = "Romero Zombie";
            Body = 3;               // Reusing the classic zombie body
            BaseSoundID = 471;      // Reusing the classic zombie sound
            Hue = UniqueHue;

            // ---- Advanced Stats ----
            SetStr(150, 200);
            SetDex(100, 125);
            SetInt(120, 140);

            SetHits(500, 600);
            SetStam(150, 200);
            SetMana(300, 350);

            SetDamage(10, 15);

            // Splitting damage: a mix of physical and energy (representing its necromantic power)
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Energy, 50);

            // Resistances
            SetResistance(ResistanceType.Physical, 40, 50);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 40, 50);
            SetResistance(ResistanceType.Poison, 100); // Essentially immune to poison
            SetResistance(ResistanceType.Energy, 40, 50);

            // Skills: a blend of martial ability and magical prowess
            SetSkill(SkillName.Magery, 80.1, 100.0);
            SetSkill(SkillName.MagicResist, 90.1, 110.0);
            SetSkill(SkillName.Tactics, 60.1, 80.0);
            SetSkill(SkillName.Wrestling, 60.1, 80.0);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 50;
            ControlSlots = 3;

            // ---- Ability Cooldowns ----
            m_NextSoulSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            m_LastLocation = this.Location;

            // Optional loot added in GenerateLoot()
        }

        // --- Withering Decay Aura (Movement Effect) ---
        // Each time Romero Zombie moves, any nearby foe (within 2 tiles) is damaged and has a portion of life drained,
        // healing Romero Zombie for half the damage dealt.
        public override void OnMovement(Mobile m, Point3D oldLocation)
        {
            if (m != null && m != this && m.Map == this.Map && m.InRange(this.Location, 2) && m.Alive && this.Alive && CanBeHarmful(m, false))
            {
                // Ensure we are only dealing with Mobile targets.
                if (m is Mobile target)
                {
                    DoHarmful(target);
                    int damage = Utility.RandomMinMax(5, 10);
                    AOS.Damage(target, this, damage, 0, 0, 0, 0, 100);

                    int heal = damage / 2;
                    this.Hits = Math.Min(this.Hits + heal, this.HitsMax);

                    target.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
                    target.PlaySound(0x1F8);
                }
            }
            base.OnMovement(m, oldLocation);
        }

        // --- OnThink: Ability Checks and Withering Miasma Movement Effect ---
        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            // --- Soul Siphon Attack ---
            if (DateTime.UtcNow >= m_NextSoulSiphonTime && this.InRange(Combatant.Location, 8))
            {
                SoulSiphonAttack();
                m_NextSoulSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }

            // --- Summon Minions ---
            if (DateTime.UtcNow >= m_NextSummonTime && this.InRange(Combatant.Location, 10))
            {
                SummonMinions();
                m_NextSummonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(30, 40));
            }

            // --- Withering Miasma ---
            // As Romero Zombie moves, it leaves behind a dangerous necromantic hazard (using NecromanticFlamestrikeTile)
            if (this.Location != m_LastLocation && this.Map != null && this.Map != Map.Internal && Utility.RandomDouble() < 0.2)
            {
                Point3D oldLoc = m_LastLocation;
                m_LastLocation = this.Location;

                if (Map.CanFit(oldLoc.X, oldLoc.Y, oldLoc.Z, 16, false, false))
                {
                    NecromanticFlamestrikeTile tile = new NecromanticFlamestrikeTile();
                    tile.Hue = UniqueHue;
                    tile.MoveToWorld(oldLoc, this.Map);
                }
                else
                {
                    int validZ = Map.GetAverageZ(oldLoc.X, oldLoc.Y);
                    if (Map.CanFit(oldLoc.X, oldLoc.Y, validZ, 16, false, false))
                    {
                        NecromanticFlamestrikeTile tile = new NecromanticFlamestrikeTile();
                        tile.Hue = UniqueHue;
                        tile.MoveToWorld(new Point3D(oldLoc.X, oldLoc.Y, validZ), this.Map);
                    }
                }
            }
            else
            {
                m_LastLocation = this.Location;
            }
        }

        // --- Unique Ability: Soul Siphon Attack ---
        // A targeted attack that drains life from its foe and heals Romero Zombie.
        public void SoulSiphonAttack()
        {
            if (Combatant == null || Map == null)
                return;

            IDamageable targetDamageable = Combatant;
            Point3D targetLocation;

            if (targetDamageable is Mobile targetMobile && CanBeHarmful(targetMobile, false))
            {
                targetLocation = targetMobile.Location;
                this.Say("*Soul Siphon!*");
                PlaySound(0x1F1);
                Effects.SendLocationParticles(EffectItem.Create(targetLocation, this.Map, EffectItem.DefaultDuration), 0x373A, 15, 20, UniqueHue, 0, 5021, 0);
                DoHarmful(targetMobile);
                int damage = Utility.RandomMinMax(25, 40);
                AOS.Damage(targetMobile, this, damage, 0, 0, 0, 0, 100);

                int heal = damage / 2;
                this.Hits = Math.Min(this.Hits + heal, this.HitsMax);

                targetMobile.SendMessage(0x22, "Your life force is being siphoned!");
                targetMobile.FixedParticles(0x376A, 10, 15, 5021, UniqueHue, 0, EffectLayer.Head);
            }
            else
            {
                targetLocation = targetDamageable.Location;
            }
        }

        // --- Unique Ability: Summon Minions ---
        // Periodically summons two lesser zombies to support Romero Zombie in battle.
        public void SummonMinions()
        {
            if (Map == null)
                return;

            this.Say("*Rise, my minions!*");
            PlaySound(0x1FB);

            for (int i = 0; i < 2; i++)
            {
                Zombie minion = new Zombie();
                minion.Team = this.Team; // Ensure minions share the same team
                int offsetX = Utility.RandomMinMax(-1, 1);
                int offsetY = Utility.RandomMinMax(-1, 1);
                Point3D spawnLocation = new Point3D(this.X + offsetX, this.Y + offsetY, this.Z);

                if (!this.Map.CanFit(spawnLocation.X, spawnLocation.Y, spawnLocation.Z, 16, false, false))
                    spawnLocation.Z = this.Map.GetAverageZ(spawnLocation.X, spawnLocation.Y);

                minion.MoveToWorld(spawnLocation, this.Map);
            }
        }

        // --- Additional Melee Effect: Decay Touch ---
        // Whenever Romero Zombie lands a melee hit, there is a 25% chance to infect the target with a poison effect.
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (defender is Mobile target && Utility.RandomDouble() < 0.25)
            {
                target.SendMessage(0x22, "The decay of Romero Zombie infects you!");
                target.ApplyPoison(this, Poison.Lesser);
                target.FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Head);
            }
        }

        // --- Death Effect: Necromantic Detonation ---
        // Upon death, Romero Zombie unleashes a burst of necromantic energy that spawns several hazardous tiles.
        public override void OnDeath(Container c)
        {
            if (Map == null)
            {
                base.OnDeath(c);
                return;
            }

            this.Say("*The curse... lingers!*");
            PlaySound(0x1F4);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3728, 20, 30, UniqueHue, 0, 5021, 0);

            int hazardsToDrop = Utility.RandomMinMax(3, 6);
            for (int i = 0; i < hazardsToDrop; i++)
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

                NecromanticFlamestrikeTile hazard = new NecromanticFlamestrikeTile();
                hazard.Hue = UniqueHue;
                hazard.MoveToWorld(hazardLocation, this.Map);
                Effects.SendLocationParticles(EffectItem.Create(hazardLocation, this.Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5021, 0);
            }

            base.OnDeath(c);
        }

        // --- Loot Generation ---
        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.HighScrolls, Utility.RandomMinMax(1, 2));

            // 3% chance for a unique necromantic artifact (replace NecroticAmulet with your actual item)
            if (Utility.RandomDouble() < 0.03)
                PackItem(new MaxxiaScroll());
        }

        // Standard immunities and affiliations.
        public override bool BleedImmune { get { return true; } }
        public override Poison PoisonImmune { get { return Poison.Regular; } }
        public override TribeType Tribe { get { return TribeType.Undead; } }
        public override OppositionGroup OppositionGroup { get { return OppositionGroup.FeyAndUndead; } }

        public RomeroZombie(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
            writer.Write(m_NextSoulSiphonTime);
            writer.Write(m_NextSummonTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextSoulSiphonTime = reader.ReadDateTime();
            m_NextSummonTime = reader.ReadDateTime();
            m_LastLocation = this.Location;
        }
    }
}
