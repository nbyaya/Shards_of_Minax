using System;
using Server;
using Server.Items;
using Server.Mobiles;
using Server.Engines.Quests;

namespace Server.Engines.Quests
{
    public class TomesOfUndoingQuest : BaseQuest
    {
        public override bool DoneOnce => true;

        public override object Title => "Tomes of Undoing";

        public override object Description => 
            "The Cult of the Broken Star believes knowledge is power—and madness is the final truth. " +
            "Deep in the Doom Dungeon, they hoard unholy grimoires penned by blood-soaked scholars.\n\n" +
            "Retrieve three *Blasphemous Grimoires* from the cult’s librarians. " +
            "Touch not their pages. Do not read them. Return them to me for incineration.\n\n" +
            "Fail in this, and their whispers will follow you until your last breath.";

        public override object Refuse =>
            "Then let the books remain where they rot. But if you start dreaming of voids and teeth… remember this moment.";

        public override object Uncomplete =>
            "I warned you not to open them. The cult librarians won’t surrender their treasures easily. " +
            "You’ll find them hunched in bone-covered alcoves, where screams go to die.";

        public override object Complete =>
            "You brought them… the ink is still wet. You didn’t *read* them, did you?\n\n" +
            "No matter. Fire cleanses all. Take this reward, and pray the Veil never lifts again.";

        public TomesOfUndoingQuest() : base()
        {
            AddObjective(new ObtainObjective(typeof(BlasphemousGrimoire), "Blasphemous Grimoire", 3, 0x0FBD, 2118)); // Hue 2118: void-scarred purple
            AddReward(new BaseReward(typeof(Gold), 9000, "9000 Gold"));
            AddReward(new BaseReward(typeof(BodySash), 1, "Witch-Hunter’s Sash"));
        }

        public override void OnCompleted()
        {
            Owner.SendMessage(0x22, "You have completed 'Tomes of Undoing'. You feel colder now. Hollow, somehow.");
            Owner.PlaySound(CompleteSound);
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
