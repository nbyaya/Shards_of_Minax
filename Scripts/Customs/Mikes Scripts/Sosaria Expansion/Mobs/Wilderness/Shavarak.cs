using System;
using System.Collections.Generic;
using Server;
using Server.Items;
using Server.Network;
using Server.Spells;        // Needed for SpellHelper
using Server.Spells.Fourth; // If you ever use curse/poison
using Server.Spells.Fifth;  // If you ever need paralyze/slow
using Server.Spells.Seventh; // For polymorph/invul/etc

namespace Server.Mobiles
{
    [CorpseName("a decaying corpse of Shavarak")]
    public class Shavarak : BaseCreature
    {
        private DateTime m_NextWhispersOfDecay;

        [Constructable]
        public Shavarak()
            : base(AIType.AI_Melee, FightMode.Closest, 10, 1, 0.2, 0.4)
        {
            Name = "Shavarak";
            Body = 154;
            BaseSoundID = 471;

            SetStr(600, 700);
            SetDex(100, 120);
            SetInt(150, 180);

            SetHits(1500, 1800);
            SetDamage(25, 35);

            SetDamageType(ResistanceType.Physical, 50);
            SetDamageType(ResistanceType.Cold, 30);
            SetDamageType(ResistanceType.Poison, 20);

            SetResistance(ResistanceType.Physical, 65, 75);
            SetResistance(ResistanceType.Fire, 20, 30);
            SetResistance(ResistanceType.Cold, 70, 80);
            SetResistance(ResistanceType.Poison, 50, 60);
            SetResistance(ResistanceType.Energy, 40, 50);

            SetSkill(SkillName.MagicResist, 100.0, 110.0);
            SetSkill(SkillName.Tactics, 95.0, 105.0);
            SetSkill(SkillName.Wrestling, 100.0, 110.0);
            SetSkill(SkillName.Healing, 60.0, 70.0);
            SetSkill(SkillName.Magery, 80.0, 90.0);

            Fame = 15000;
            Karma = -15000;

            VirtualArmor = 60;

            Hue = 0x481;
        }

        public Shavarak(Serial serial)
            : base(serial)
        {
        }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant != null && !Combatant.Deleted && Combatant.Map == this.Map && DateTime.UtcNow >= m_NextWhispersOfDecay)
            {
                var targets = new List<Mobile>();
                var map = this.Map;

                if (map != null)
                {
                    foreach (Mobile m in map.GetMobilesInRange(this.Location, 8))
                    {
                        if (m != this && SpellHelper.ValidIndirectTarget(this, m) && this.CanBeHarmful(m, false))
                            targets.Add(m);
                    }
                }

                if (targets.Count > 1 || (targets.Count == 1 && targets[0] == Combatant))
                {
                    WhispersOfDecay();
                }
            }
        }

        public void WhispersOfDecay()
        {
            if (DateTime.UtcNow < m_NextWhispersOfDecay)
                return;

            PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*Shavarak begins to channel ancient decay...*");
            PlaySound(0x108);
            Animate(10, 5, 1, true, false, 0);

            Timer.DelayCall(TimeSpan.FromSeconds(4.0), () =>
            {
                PublicOverheadMessage(MessageType.Regular, 0x3B2, false, "*A wave of decay washes over the area!*");
                PlaySound(0x214);

                var map = this.Map;
                if (map == null) return;

                var targets = new List<Mobile>();
                foreach (Mobile m in map.GetMobilesInRange(this.Location, 8))
                {
                    if (m != this && SpellHelper.ValidIndirectTarget(this, m) && this.CanBeHarmful(m, false))
                        targets.Add(m);
                }

                foreach (Mobile target in targets)
                {
                    if (target == null || target.Deleted) continue;

                    int damage = Utility.RandomMinMax(30, 50);
                    AOS.Damage(target, this, damage, 50, 0, 50, 0, 0);

                    if (Utility.RandomDouble() < 0.33)
                    {
                        target.SendLocalizedMessage(1072318); // You feel your defenses weaken!
                        var mod = new ResistanceMod(ResistanceType.Physical, -10);
                        target.AddResistanceMod(mod);

                        Timer.DelayCall(TimeSpan.FromSeconds(10.0), () =>
                        {
                            if (target != null && !target.Deleted)
                            {
                                target.RemoveResistanceMod(mod);
                                target.SendLocalizedMessage(1072319); // You feel your defenses return to normal.
                            }
                        });
                    }
                }

                m_NextWhispersOfDecay = DateTime.UtcNow + TimeSpan.FromSeconds(20.0 + (10.0 * Utility.RandomDouble()));
            });
        }

        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.25)
            {
                defender.SendAsciiMessage("Shavarak's touch saps your energy!");
                PlaySound(0x44A);

                int drain = Utility.RandomMinMax(10, 20);
                defender.Stam -= drain;
                defender.Mana -= drain;

                defender.Frozen = true;
                Timer.DelayCall(TimeSpan.FromSeconds(2.0), () =>
                {
                    if (defender != null && !defender.Deleted)
                        defender.Frozen = false;
                });
            }
        }

        public override int TreasureMapLevel => 4;
        public override bool BleedImmune => true;
        public override Poison PoisonImmune => Poison.Greater;
        public override TribeType Tribe => TribeType.Undead;
        public override OppositionGroup OppositionGroup => OppositionGroup.FeyAndUndead;

        public override void GenerateLoot()
        {
            AddLoot(LootPack.UltraRich);
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Potions);

            if (Utility.RandomDouble() < 0.005)
                PackItem(new PharaohsGoldenCrown());
        }

        public override void Serialize(GenericWriter writer)
        {
            base.Serialize(writer);
            writer.Write((int)0);
        }

        public override void Deserialize(GenericReader reader)
        {
            base.Deserialize(reader);
            int version = reader.ReadInt();
        }
    }
}
