using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Spells;      // For potential spell effects
using Server.Network;     // For visual and sound effects
using System.Collections.Generic;

namespace Server.Mobiles
{
    [CorpseName("a mutant lab rat corpse")]
    public class LabRat : BaseCreature
    {
        // Ability cooldown timers
        private DateTime m_NextSqueakTime;
        private DateTime m_NextAmbushTime;
        private DateTime m_NextSiphonTime;

        // Unique Hue for the Lab Rat
        private const int UniqueHue = 1175;  // Chosen for a distinct arcane green tint

        [Constructable]
        public LabRat() : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "a Lab Rat";
            Body = 0xD7;           // Same as the Giant Rat's body
            BaseSoundID = 0x188;   // Same as the Giant Rat's sound
            Hue = UniqueHue;

            // Advanced Stats (much higher than a normal giant rat)
            SetStr(200, 250);
            SetDex(150, 200);
            SetInt(250, 300);

            SetHits(800, 1000);
            SetStam(300, 350);
            SetMana(400, 500);

            SetDamage(10, 15);     // Base physical damage

            // Damage types: Split between Physical and Poison
            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Poison, 50);

            // Resistances – with a particular emphasis on poison
            SetResistance(ResistanceType.Physical, 30, 40);
            SetResistance(ResistanceType.Fire, 30, 40);
            SetResistance(ResistanceType.Cold, 30, 40);
            SetResistance(ResistanceType.Poison, 70, 80);
            SetResistance(ResistanceType.Energy, 30, 40);

            // Skills – tuned for a magic–themed creature that can dish out area effects
            SetSkill(SkillName.EvalInt, 100, 110);
            SetSkill(SkillName.Magery, 100, 110);
            SetSkill(SkillName.MagicResist, 110, 120);
            SetSkill(SkillName.Tactics, 90, 100);
            SetSkill(SkillName.Wrestling, 90, 100);

            Fame = 12000;
            Karma = -12000;

            VirtualArmor = 50;
            ControlSlots = 3;

            // Initialize ability cooldowns (in seconds)
            m_NextSqueakTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(8, 12));
            m_NextAmbushTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
        }

        public override void OnThink()
        {
            base.OnThink();

            if (Combatant == null || Map == null || !Alive)
                return;

            // Prioritize abilities based on their cooldowns and range:
            if (DateTime.UtcNow >= m_NextSqueakTime && this.InRange(Combatant.Location, 4))
            {
                PlagueSqueakAttack();
                m_NextSqueakTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(10, 15));
            }
            else if (DateTime.UtcNow >= m_NextAmbushTime && this.InRange(Combatant.Location, 8))
            {
                LabAmbushAttack();
                m_NextAmbushTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 20));
            }
            else if (DateTime.UtcNow >= m_NextSiphonTime && this.InRange(Combatant.Location, 10))
            {
                MutagenicSiphonAttack();
                m_NextSiphonTime = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(20, 25));
            }
        }

        // Ability 1: Plague Squeak Attack
        // An AoE attack that emits a toxic squeak, dealing poison damage and potentially infecting targets.
        public void PlagueSqueakAttack()
        {
            this.Say("*Squeak of pestilence!*");
            PlaySound(0x188); // Base rat sound – could be replaced with a custom sound if desired.
            FixedParticles(0x3709, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);

            List<Mobile> targets = new List<Mobile>();
            IPooledEnumerable eable = Map.GetMobilesInRange(this.Location, 4);
            foreach (Mobile m in eable)
            {
                if (m != this && CanBeHarmful(m, false) && SpellHelper.ValidIndirectTarget(this, m))
                    targets.Add(m);
            }
            eable.Free();

            foreach (Mobile target in targets)
            {
                DoHarmful(target);
                int damage = Utility.RandomMinMax(15, 25);
                // Deal 100% poison damage.
                AOS.Damage(target, this, damage, 0, 0, 100, 0, 0);

                // 30% chance to apply poison.
                if (Utility.RandomDouble() < 0.3)
                {
                    if (target is Mobile targetMobile)
                        targetMobile.ApplyPoison(this, Poison.Regular);
                }
            }
        }

        // Ability 2: Lab Ambush Attack
        // The Lab Rat quickly teleports adjacent to its target and spawns a toxic hazard (ToxicGasTile) at the target's previous location.
        public void LabAmbushAttack()
        {
            if (Combatant is Mobile target)
            {
                DoHarmful(target);
                this.Say("*Skittering into the shadows!*");
                PlaySound(0x20E); // Teleport sound

                // Teleport to a location immediately adjacent to the target.
                Point3D targetLocation = target.Location;
                int offsetX = Utility.RandomMinMax(-1, 1);
                int offsetY = Utility.RandomMinMax(-1, 1);
                Point3D newLocation = new Point3D(targetLocation.X + offsetX, targetLocation.Y + offsetY, targetLocation.Z);
                if (Map.CanFit(newLocation.X, newLocation.Y, newLocation.Z, 16, false, false))
                {
                    this.Location = newLocation;
                    FixedParticles(0x376A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
                }

                // After a brief delay, spawn a ToxicGasTile hazard at the target's former location.
                Timer.DelayCall(TimeSpan.FromSeconds(0.5), () =>
                {
                    if (Map == null) return;
                    Point3D spawnLoc = targetLocation;
                    if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                    {
                        spawnLoc.Z = Map.GetAverageZ(spawnLoc.X, spawnLoc.Y);
                        if (!Map.CanFit(spawnLoc.X, spawnLoc.Y, spawnLoc.Z, 16, false, false))
                            return;
                    }
                    ToxicGasTile gasTile = new ToxicGasTile();  // Assumes ToxicGasTile is defined elsewhere.
                    gasTile.Hue = UniqueHue;
                    gasTile.MoveToWorld(spawnLoc, Map);
                    Effects.PlaySound(spawnLoc, Map, 0x1F6);  // A gas release sound.
                });
            }
        }

        // Ability 3: Mutagenic Siphon Attack
        // A focused drain effect that zaps a target, dealing damage while draining health and mana, and healing the Lab Rat.
        public void MutagenicSiphonAttack()
        {
            if (Combatant is Mobile target)
            {
                DoHarmful(target);
                this.Say("*Absorbing your vitality!*");
                PlaySound(0x1F0); // Siphon sound effect

                int damage = Utility.RandomMinMax(20, 35);
                // Deal physical damage (100% physical).
                AOS.Damage(target, this, damage, 100, 0, 0, 0, 0);

                // Drain some mana.
                int manaDrain = Utility.RandomMinMax(15, 30);
                if (target.Mana >= manaDrain)
                {
                    target.Mana -= manaDrain;
                    target.SendMessage(0x22, "The Lab Rat siphons your magical energy!");
                }
                else
                {
                    manaDrain = target.Mana;
                    target.Mana = 0;
                    target.SendMessage(0x22, "The Lab Rat drains the last of your magical energy!");
                }

                // Heal self by a portion of the damage and mana drained.
                int healAmount = (damage + manaDrain) / 2;
                this.Hits += healAmount;
                if (this.Hits > HitsMax)
                    this.Hits = HitsMax;
                FixedParticles(0x375A, 10, 15, 5032, UniqueHue, 0, EffectLayer.Waist);
            }
        }

        // Override the death behavior to provide a dramatic toxic explosion.
        public override void OnDeath(Container c)
        {
            if (Map != null)
            {
                this.Say("*The toxic burst of a lab experiment gone wrong!*");
                PlaySound(0x20C); // Explosion sound effect
                Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, EffectItem.DefaultDuration), 0x3709, 10, 30, UniqueHue, 0, 5052, 0);

                // Spawn several ToxicGasTiles around the corpse.
                int hazardsToDrop = Utility.RandomMinMax(3, 5);
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
                    ToxicGasTile gasTile = new ToxicGasTile();
                    gasTile.Hue = UniqueHue;
                    gasTile.MoveToWorld(hazardLocation, Map);
                    Effects.SendLocationParticles(EffectItem.Create(hazardLocation, Map, EffectItem.DefaultDuration), 0x376A, 10, 20, UniqueHue, 0, 5039, 0);
                }
            }

            base.OnDeath(c);
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Rich, 2);
            AddLoot(LootPack.MedScrolls, 1);
            AddLoot(LootPack.Gems, Utility.RandomMinMax(3, 6));
        }

        public override bool BleedImmune { get { return true; } }
        public override int Meat { get { return 1; } }
        public override int Hides { get { return 6; } }
        public override FoodType FavoriteFood { get { return FoodType.Fish | FoodType.Meat | FoodType.FruitsAndVegies | FoodType.Eggs; } }

        public LabRat(Serial serial) : base(serial)
        {
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);  // version
            writer.Write(m_NextSqueakTime);
            writer.Write(m_NextAmbushTime);
            writer.Write(m_NextSiphonTime);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
            m_NextSqueakTime = reader.ReadDateTime();
            m_NextAmbushTime = reader.ReadDateTime();
            m_NextSiphonTime = reader.ReadDateTime();
        }
    }
}
