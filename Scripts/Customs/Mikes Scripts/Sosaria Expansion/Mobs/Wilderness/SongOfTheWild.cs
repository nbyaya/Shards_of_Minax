using System;
using System.Collections.Generic;
using Server;
using Server.Mobiles;
using Server.Items;

namespace Server.Mobiles
{
    [CorpseName("the corpse of a Song of the Wild")]
    public class SongOfTheWild : BaseCreature
    {
        private DateTime _NextWindBurst;
        private DateTime _NextNatureSong;
        private DateTime _NextEntanglingRoots;
        private DateTime _NextPoisonSong;

        [Constructable]
        public SongOfTheWild()
            : base(AIType.AI_Mage, FightMode.Closest, 12, 1, 0.3, 0.6)
        {
            Name           = "Song of the Wild";
            Body           = 6;       // bird body
            BaseSoundID    = 0x1B;    // bird call
            Hue            = 0x88F;   // unique emerald‐tinged hue

            // Stats
            SetStr(300, 350);
            SetDex(150, 180);
            SetInt(400, 450);

            SetHits(800, 900);
            SetMana(500, 600);

            SetDamage(15, 25);

            // Damage types
            SetDamageType(ResistanceType.Physical, 30);
            SetDamageType(ResistanceType.Cold,      30);
            SetDamageType(ResistanceType.Fire,      20);
            SetDamageType(ResistanceType.Energy,    20);

            // Resistances
            SetResistance(ResistanceType.Physical, 50, 60);
            SetResistance(ResistanceType.Fire,     40, 50);
            SetResistance(ResistanceType.Cold,     40, 50);
            SetResistance(ResistanceType.Poison,   50, 60);
            SetResistance(ResistanceType.Energy,   50, 60);

            // Skills
            SetSkill(SkillName.EvalInt,     100.0);
            SetSkill(SkillName.Magery,      110.0);
            SetSkill(SkillName.Meditation,  120.0);
            SetSkill(SkillName.MagicResist, 125.0);
            SetSkill(SkillName.Tactics,      85.0);
            SetSkill(SkillName.Wrestling,    80.0);

            Fame           = 10000;
            Karma          = -10000;
            VirtualArmor   = 40;

            Tamable        = false;
            ControlSlots   = 2;
        }

        public SongOfTheWild(Serial serial) : base(serial) { }

        public override void OnActionCombat()
        {
            base.OnActionCombat();

            if (Combatant is Mobile target && !target.Deleted && target.Map == Map && InLOS(target))
            {
                var now = DateTime.UtcNow;

                if (now >= _NextWindBurst && InRange(target, 12))
                {
                    WindBurst(target);
                    _NextWindBurst = now + TimeSpan.FromSeconds(15.0);
                }
                else if (now >= _NextNatureSong)
                {
                    NatureSong();
                    _NextNatureSong = now + TimeSpan.FromSeconds(20.0);
                }
                else if (now >= _NextEntanglingRoots && InRange(target, 8))
                {
                    EntanglingRoots(target);
                    _NextEntanglingRoots = now + TimeSpan.FromSeconds(25.0);
                }
                else if (now >= _NextPoisonSong)
                {
                    PoisonSong();
                    _NextPoisonSong = now + TimeSpan.FromSeconds(30.0);
                }
            }
        }

        // 1) Wind‐up AOE burst: flurry of feathers plus splash damage
        private void WindBurst(Mobile target)
        {
            Animate(12, 5, 1, true, false, 0);   // wing‐flap animation
            PlaySound(0x20B);
            // ← FIXED: pass the target as IEntity, not its Location
            Effects.SendMovingEffect(this, target, 0x1FBD, 10, 1, false, false);

            Timer.DelayCall(TimeSpan.FromSeconds(1.0), () =>
            {
                if (target != null && CanBeHarmful(target))
                {
                    DoHarmful(target);
                    AOS.Damage(target, this, Utility.RandomMinMax(30, 45), 100, 0, 0, 0, 0);

                    // AoE splash
                    foreach (var m in target.GetMobilesInRange(2))
                    {
                        if (m != this && CanBeHarmful(m))
                        {
                            DoHarmful(m);
                            AOS.Damage(m, this, Utility.RandomMinMax(20, 35), 100, 0, 0, 0, 0);
                        }
                    }
                }
            });
        }

        // 2) Nature’s Song: heals allies, wounds foes in 6‐tile radius
        private void NatureSong()
        {
            Animate(11, 5, 1, true, false, 0);  // singing pose
            PlaySound(0x3E9);

            foreach (var m in GetMobilesInRange(6))
            {
                if (CanBeBeneficial(m))
                {
                    m.Heal(Utility.RandomMinMax(20, 35));
                    m.SendMessage("You feel renewed by the Song of the Wild.");
                }
                else if (CanBeHarmful(m))
                {
                    DoHarmful(m);
                    AOS.Damage(m, this, Utility.RandomMinMax(10, 20), 0, 0, 0, 0, 100);
                    m.SendMessage("The Song of the Wild rends your spirit!");
                }
            }
        }

        // 3) Entangling Roots: roots and stuns a single target
        private void EntanglingRoots(Mobile target)
        {
            Animate(13, 5, 1, true, false, 0);
            PlaySound(0x6C2);
            // ← FIXED: added a seventh argument and switched to IEntity overload


            if (target != null && CanBeHarmful(target))
            {
                DoHarmful(target);
                target.Freeze(TimeSpan.FromSeconds(4.0));
                target.SendMessage("Vines erupt from the earth, binding you!");
            }
        }

        // 4) Poison Song: clouds area with toxic melody
        private void PoisonSong()
        {
            Animate(11, 5, 1, true, false, 0);
            PlaySound(0x1F1);

            foreach (var m in GetMobilesInRange(8))
            {
                if (m is Mobile mv && CanBeHarmful(mv))
                {
                    mv.ApplyPoison(this, Poison.Lesser);
                    mv.SendMessage("You inhale a toxic crescendo!");
                }
            }
        }

        // Melee stun/extra‐damage on hit
        public override void OnGaveMeleeAttack(Mobile defender)
        {
            base.OnGaveMeleeAttack(defender);

            if (Utility.RandomDouble() < 0.3)
            {
                defender.SendMessage("The Song of the Wild’s talons rend your flesh!");
                AOS.Damage(defender, this, Utility.RandomMinMax(10, 20), 100, 0, 0, 0, 0);
            }
        }

        public override void GenerateLoot()
        {
            AddLoot(LootPack.Gems);
            AddLoot(LootPack.Rich);
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
