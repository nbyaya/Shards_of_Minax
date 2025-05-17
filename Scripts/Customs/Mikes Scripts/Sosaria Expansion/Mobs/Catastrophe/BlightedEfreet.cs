using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Network;

namespace Server.Mobiles
{
    [CorpseName("a blighted efreet corpse")]
    public class BlightedEfreet : BaseCreature
    {
        private DateTime m_NextBlightCloud;
        private DateTime m_NextManaDrain;
        private bool m_HasFlameBursted;

        [Constructable]
        public BlightedEfreet()
            : base(AIType.AI_Mage, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            this.Name = "a blighted efreet";
            this.Body = 131;
            this.BaseSoundID = 768;
            this.Hue = 0x835; // Unique hue distinguishing the blight variant



            // Enhanced attributes
            this.SetStr(400, 420);
            this.SetDex(300, 320);
            this.SetInt(250, 270);

            this.SetHits(250, 280);

            this.SetDamage(15, 20);

            // Damage mix: a blend of physical, fire, and energy
            this.SetDamageType(ResistanceType.Physical, 20);
            this.SetDamageType(ResistanceType.Fire, 60);
            this.SetDamageType(ResistanceType.Energy, 20);

            // Resistances
            this.SetResistance(ResistanceType.Physical, 55, 65);
            this.SetResistance(ResistanceType.Fire, 80, 90);
            this.SetResistance(ResistanceType.Poison, 40, 50);
            this.SetResistance(ResistanceType.Energy, 50, 60);

            // Skills boosted for an advanced magical creature
            this.SetSkill(SkillName.EvalInt, 80.1, 90.0);
            this.SetSkill(SkillName.Magery, 80.1, 90.0);
            this.SetSkill(SkillName.MagicResist, 80.1, 90.0);
            this.SetSkill(SkillName.Tactics, 70.1, 80.0);
            this.SetSkill(SkillName.Wrestling, 70.1, 80.0);

            // Fame, Karma, and Virtual Armor
            this.Fame = 15000;
            this.Karma = -15000;
            this.VirtualArmor = 70;

            // Initialize special ability cooldown timers
            m_NextBlightCloud = DateTime.UtcNow + TimeSpan.FromSeconds(10);
            m_NextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(15);
        }

        public override void OnActionCombat()
        {
            Mobile combatant = Combatant as Mobile;
            if (combatant == null || combatant.Deleted || combatant.Map != Map || 
                !InRange(combatant, 12) || !CanBeHarmful(combatant) || !InLOS(combatant))
            {
                return;
            }

            // Blight Cloud Ability: Releases a toxic cloud that damages the target over time.
            if (DateTime.UtcNow >= m_NextBlightCloud)
            {
                BlightCloud(combatant);
                m_NextBlightCloud = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(12, 18));
            }

            // Mana Drain Ability: Steals mana from the target to heal itself.
            if (DateTime.UtcNow >= m_NextManaDrain)
            {
                DrainMana(combatant);
                m_NextManaDrain = DateTime.UtcNow + TimeSpan.FromSeconds(Utility.RandomMinMax(15, 25));
            }

            // Corrupted Flame Burst: Triggered once when its health drops below 50%.
            if (!m_HasFlameBursted && Hits < HitsMax / 2)
            {
                m_HasFlameBursted = true;
                CorruptedFlameBurst();
            }
        }

        private void BlightCloud(Mobile target)
        {
            // Validate the target; ensure it's a Mobile
            if (target == null || target.Deleted)
                return;
            if (!(target is Mobile))
                return;

            DoHarmful(target);
            // Visual and sound effects to convey the toxic cloud
            Effects.SendLocationParticles(EffectItem.Create(target.Location, target.Map, TimeSpan.FromSeconds(0.5)),
                                          0x3709, 10, 30, 0x835, 0, 5029, 0);
            target.PlaySound(0x5C3);

            // Damage over time effect: 5 damage per tick for 3 ticks.
            int ticks = 3;
            int damagePerTick = 5;

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target != null && !target.Deleted && CanBeHarmful(target))
                {
                    for (int i = 0; i < ticks; i++)
                    {
                        Timer.DelayCall(TimeSpan.FromSeconds(i + 1), () =>
                        {
                            if (target != null && !target.Deleted && CanBeHarmful(target))
                            {
                                AOS.Damage(target, this, damagePerTick, 0, 100, 0, 0, 0);
                                target.SendAsciiMessage("The toxic blight eats away at your strength!");
                            }
                        });
                    }
                }
            });
        }

        private void DrainMana(Mobile target)
        {
            // Validate the target is a Mobile before accessing Mana
            if (target == null || target.Deleted)
                return;
            if (!(target is Mobile))
                return;

            DoHarmful(target);
            // Drain a random amount of mana between 10 and 20.
            int manaDrain = Utility.RandomMinMax(10, 20);
            if (target.Mana >= manaDrain)
            {
                target.Mana -= manaDrain;
                // Heal self for the drained amount (ensuring not to exceed maximum health)
                this.Hits += manaDrain;
                if (this.Hits > this.HitsMax)
                    this.Hits = this.HitsMax;
                target.SendAsciiMessage("You feel your magical energy being sapped!");
                this.PublicOverheadMessage(MessageType.Regular, 0x22, false, "*" + manaDrain + " mana drained*");
            }
        }

        private void CorruptedFlameBurst()
        {
            // Announce the burst with visual and sound effects
            this.PlaySound(0x208);
            Effects.SendLocationParticles(EffectItem.Create(this.Location, this.Map, TimeSpan.FromSeconds(0.5)),
                                          0x36BD, 20, 30, 0x835, 0, 5029, 0);

            // Damage all nearby enemies within a 3-tile radius.
            List<Mobile> targets = new List<Mobile>();
            foreach (Mobile m in this.GetMobilesInRange(3))
            {
                if (m == this || !CanBeHarmful(m))
                    continue;
                targets.Add(m);
            }

            foreach (Mobile m in targets)
            {
                DoHarmful(m);
                AOS.Damage(m, this, Utility.RandomMinMax(20, 30), 0, 100, 0, 0, 0);
                m.SendAsciiMessage("You are scorched by a burst of corrupted flame!");
            }
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);
            // Chance for an additional withering touch on melee strikes.
            if (0.3 > Utility.RandomDouble())
            {
                if (defender != null && !defender.Deleted && CanBeHarmful(defender))
                {
                    defender.SendAsciiMessage("The blighted efreet's withering touch drains your vitality!");
                    AOS.Damage(defender, this, Utility.RandomMinMax(10, 15), 0, 100, 0, 0, 0);
                }
            }
        }

        public override int TreasureMapLevel { get { return 5; } }

        public override void GenerateLoot()
        {
            // Advanced loot generation: richer rewards and a rare corrupted artifact.
            this.AddLoot(LootPack.FilthyRich);
            this.AddLoot(LootPack.Rich);
            this.AddLoot(LootPack.Gems, Utility.RandomMinMax(2, 4));

        }

        public BlightedEfreet(Serial serial)
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
        }
    }
}
